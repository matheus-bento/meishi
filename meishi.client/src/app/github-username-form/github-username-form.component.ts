import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Component, signal } from '@angular/core';
import { GithubUserData } from '../../core/model/github-user-data';

@Component({
  selector: 'github-username-form',
  standalone: false,
  templateUrl: './github-username-form.component.html',
  styleUrl: './github-username-form.component.css'
})
export class GithubUsernameFormComponent {
  constructor(private http: HttpClient) {}

  githubUserData = signal<GithubUserData | null>(null);

  githubDataForm = new FormGroup({
    githubUsername: new FormControl('', Validators.required),
  });

  onSubmit() {
    this.http.get<GithubUserData>('/api/user/' + this.githubDataForm.value.githubUsername)
      .subscribe({
        next: data => this.githubUserData.set(data),
        error: _ => this.githubUserData.set(null)
      });
  }

  getGithubUserData(): GithubUserData {
    return this.githubUserData() as GithubUserData;
  }
}
