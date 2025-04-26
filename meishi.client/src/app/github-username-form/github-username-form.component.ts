import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Component } from '@angular/core';

interface GithubUserData {
  id: number;
  login: string;
  name: string;
  company: string;
  notification_email: string;
  location: string;
  bio: string;
  followers: number;
}

@Component({
  selector: 'github-username-form',
  standalone: false,
  templateUrl: './github-username-form.component.html',
  styleUrl: './github-username-form.component.css'
})
export class GithubUsernameFormComponent {
  constructor(private http: HttpClient) {}

  githubUserData: GithubUserData | null = null;

  githubDataForm = new FormGroup({
    githubUsername: new FormControl('', Validators.required),
  });

  onSubmit() {
    this.http.get<GithubUserData>('/api/user/' + this.githubDataForm.value.githubUsername)
      .subscribe(data => this.githubUserData = data)
  }
}
