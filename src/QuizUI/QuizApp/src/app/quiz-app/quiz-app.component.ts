import { Component, OnInit } from '@angular/core';
import { QuizProviderService } from '../quiz-provider.service';
import { Observable } from 'rxjs/Observable';
import { Quiz } from '../quiz';
import { Router } from '@angular/router';
import { AttemptProviderService } from '../attempt-provider.service';

@Component({
  selector: 'app-quiz-app',
  templateUrl: './quiz-app.component.html',
  styleUrls: ['./quiz-app.component.css']
})
export class QuizAppComponent implements OnInit {
  private _quizes: Quiz[] = null;
  private _email: string = null;

  constructor(private router: Router,
              private quizProvider: QuizProviderService,
              private attemptProvider: AttemptProviderService) { }

  ngOnInit() {}

  get quizes(): Quiz[] {
    return this._quizes;
  }

  get quizesrx(): Observable<Quiz[]> {
    return this.quizProvider.AvailableQuizes;
  }

  get email(): string {
    return this._email;
  }

  set email(email: string) {
    console.log('new email set: ' + email);
    this._email = email;
  }

  startAttempt(email: string, quizid: string) {
    this.attemptProvider.StartAttempt(email, quizid);
    this.router.navigate(['/attempt']);
  }
}
