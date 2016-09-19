import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/rx';

import { AttemptProviderService } from './attempt-provider.service';
import { Attempt, Question, Choice } from './attempt';

@Injectable()
export class AttemptNavigatorService {
  private _attempt: Attempt = null;
  private _currentQuestionIndex: number = 0;
  private _currentQuestion: BehaviorSubject<Question> = null;

  constructor(private attemptProvider: AttemptProviderService) {
    attemptProvider.CurrentAttempt.subscribe(
      a => this._attempt = a,
      err => console.log('error received from attemptprovider: ' + err),
      () => console.log('attemptprovider currentattempt observable finished') );
  }

  get CurrentQuestion(): Observable<Question> {
    return this._currentQuestion;
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
