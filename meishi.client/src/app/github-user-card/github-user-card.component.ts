import { Component, input } from '@angular/core';
import { GithubUserData } from '../../core/model/github-user-data';

@Component({
  selector: 'app-github-user-card',
  standalone: false,
  templateUrl: './github-user-card.component.html',
  styleUrl: './github-user-card.component.css'
})
export class GithubUserCardComponent {
  githubUser = input.required<GithubUserData>();
}
