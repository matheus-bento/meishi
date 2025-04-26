import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GithubUsernameFormComponent } from './github-username-form.component';

import { By } from '@angular/platform-browser';

import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

describe('GithubUsernameFormComponent', () => {
  let component: GithubUsernameFormComponent;
  let fixture: ComponentFixture<GithubUsernameFormComponent>;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GithubUsernameFormComponent],
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

  it('should create the app', () => {
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
    const mockGithubUserData = {
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

    component.onSubmit();

    const req = httpMock.expectOne('/api/user/' + mockGithubUserData.login);
    expect(req.request.method).toEqual('GET');

    req.flush(mockGithubUserData);

    expect(component.githubUserData).toEqual(mockGithubUserData);
  });
});
