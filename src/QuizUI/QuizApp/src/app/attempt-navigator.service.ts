import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Subject, BehaviorSubject } from 'rxjs/rx';

import { AttemptProviderService } from './attempt-provider.service';
import { Attempt, Question, Choice } from './attempt';

@Injectable()
export class AttemptNavigatorService {
  private _attempt: Attempt = null;
  private _currentQuestionIndex: number = 0;
  private _currentQuestion: Subject<Question> = new Subject<Question>();

  constructor(private attemptProvider: AttemptProviderService) {
    attemptProvider.CurrentAttempt.subscribe(
      a => {this._attempt = a; this.MoveToNextQuestion(); },
      err => console.log('error received from attemptprovider: ' + err),
      () => console.log('attemptprovider currentattempt observable finished') );
    console.log('subscribed to attemptprovider service');
  }

  get CurrentQuestion(): Observable<Question> {
    return this._currentQuestion
               .do(q => console.log('current question set to: ' + q.Question));
  }

  MoveToNextQuestion() {
    if (this._currentQuestionIndex < (this._attempt.Questions.length - 1)) {
      this._currentQuestionIndex++;
      this._currentQuestion.next(this._attempt.Questions[this._currentQuestionIndex]);
    }
  }

  MoveToPreviousQuestion() {
    if (this._currentQuestionIndex > 0) {
      this._currentQuestionIndex--;
      this._currentQuestion.next(this._attempt.Questions[this._currentQuestionIndex]);
    }
  }

}
