import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

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
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent {
  constructor(private http: HttpClient) {}

  githubUserData: GithubUserData | null = null;

  githubDataForm = new FormGroup({
    githubUsername: new FormControl('', Validators.required),
  });

  onSubmit() {
    this.http.get<GithubUserData>(`/user/${this.githubDataForm.value.githubUsername}`)
      .subscribe(data => this.githubUserData = data)
  }
}
