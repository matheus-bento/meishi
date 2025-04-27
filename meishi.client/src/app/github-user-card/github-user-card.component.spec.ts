import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GithubUserCardComponent } from './github-user-card.component';
import { GithubUserData } from '../../core/model/github-user-data';

describe('GithubUserCardComponent', () => {
  let component: GithubUserCardComponent;
  let fixture: ComponentFixture<GithubUserCardComponent>;
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

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GithubUserCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GithubUserCardComponent);
    fixture.componentRef.setInput('githubUser', mockGithubUserData);

    component = fixture.componentInstance;
    
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
