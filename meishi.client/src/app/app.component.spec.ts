import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { By } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AppComponent],
      imports: [ReactiveFormsModule],
      providers: [
        provideHttpClient
          (withFetch()
        ),
        provideHttpClientTesting()
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
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