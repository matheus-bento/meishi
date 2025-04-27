import { HttpTestingController, provideHttpClientTesting, TestRequest } from '@angular/common/http/testing';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GithubUsernameFormComponent } from './github-username-form.component';

import { By } from '@angular/platform-browser';

import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { DebugElement } from '@angular/core';
import { GithubUserData } from '../../core/model/github-user-data';
import { GithubUserCardComponent } from '../github-user-card/github-user-card.component';

describe('GithubUsernameFormComponent', () => {
  let component: GithubUsernameFormComponent;
  let fixture: ComponentFixture<GithubUsernameFormComponent>;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        GithubUsernameFormComponent,
        GithubUserCardComponent
      ],
      imports: [
        MatButtonModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule
      ],
      providers: [
        provideHttpClient
          (withFetch()
        ),
        provideHttpClientTesting()
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GithubUsernameFormComponent);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should disable the submit button on component startup', () => {
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.css('button')).properties['disabled']).toBeTrue();
  });

  it('should disable the submit button if github username input is empty', () => {
    component.githubDataForm.setValue({ githubUsername: '' });
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.css('button')).properties['disabled']).toBeTrue();
  });

  it('should enable the submit button if github username input is filled', () => {
    component.githubDataForm.setValue({ githubUsername: 'something' });
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.css('button')).properties['disabled']).toBeFalse();
  });

  it('should retrieve the github user data from the server', () => {
    const mockGithubUserData: GithubUserData = {
      id: 99999999,
      login: "mock-user",
      name: "Mock User",
      company: "Mock Company",
      notification_email: "mock-user@mock.com",
      location: "Rio de Janeiro, Brasil",
      bio: "Just a mock user",
      followers: 10
    };

    component.githubDataForm.setValue({
      githubUsername: mockGithubUserData.login
    });
    fixture.detectChanges();

    component.onSubmit();

    const req: TestRequest = httpMock.expectOne('/api/user/' + mockGithubUserData.login);
    expect(req.request.method).toEqual('GET');

    req.flush(mockGithubUserData);

    expect(component.githubUserData()).toEqual(mockGithubUserData);
  });

  it('should not load GithubUserCardComponent on creation', () => {
    fixture.detectChanges();
    const githubUserCard = fixture.debugElement.query(By.css('app-github-user-card'));
    expect(githubUserCard).toBeNull();
  });

  it('should not load GithubUserCardComponent if githubUserData is null', () => {
    component.githubUserData.set(null);
    fixture.detectChanges();

    const githubUserCard = fixture.debugElement.query(By.css('app-github-user-card'));
    
    expect(githubUserCard).toBeNull();
  });

  it('should hide GithubUserCardComponent if github user is not found', () => {
    let githubUserCard: DebugElement | null = null;

    // Initial request with existing user
    const mockGithubUserData: GithubUserData = {
      id: 99999999,
      login: "mock-user",
      name: "Mock User",
      company: "Mock Company",
      notification_email: "mock-user@mock.com",
      location: "Rio de Janeiro, Brasil",
      bio: "Just a mock user",
      followers: 10
    };

    component.githubDataForm.setValue({
      githubUsername: mockGithubUserData.login
    });
    fixture.detectChanges();

    component.onSubmit();

    const reqExistingUser: TestRequest = httpMock.expectOne('/api/user/' + mockGithubUserData.login);
    expect(reqExistingUser.request.method).toEqual('GET');

    reqExistingUser.flush(mockGithubUserData);
    fixture.detectChanges();

    githubUserCard = fixture.debugElement.query(By.css('app-github-user-card'));

    expect(githubUserCard).not.toBeNull();
    
    // Second request with non-existing user
    
    component.githubDataForm.setValue({
      githubUsername: "non-existing-user"
    });
    fixture.detectChanges();

    component.onSubmit();

    const reqNonExistingUser: TestRequest = httpMock.expectOne('/api/user/' + component.githubDataForm.value.githubUsername);
    expect(reqNonExistingUser.request.method).toEqual('GET');

    reqNonExistingUser.error(new ProgressEvent('error'), { status: 404 });
    fixture.detectChanges();

    githubUserCard = fixture.debugElement.query(By.css('app-github-user-card'));

    expect(githubUserCard).toBeNull();
  });
});
