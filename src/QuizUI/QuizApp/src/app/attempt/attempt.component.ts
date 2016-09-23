import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { AttemptNavigatorService } from '../attempt-navigator.service';
import { AnswerCollectionService } from '../answer-collection.service';
import { Question, Choice } from '../attempt';

@Component({
  selector: 'app-attempt',
  templateUrl: './attempt.component.html',
  styleUrls: ['./attempt.component.css'],
  providers: [AnswerCollectionService]
})
export class AttemptComponent implements OnInit {

  currentQuestion: Observable<string> = null;
  currentQuestionId: Observable<string> = null;
  currentQuestionChoices: Observable<string[]> = null;
  currentAnswer: number = null;
  canMoveToNextQuestion: Observable<boolean> = null;

  private _currentQuestionIdForAnswer: string = null;

  constructor(private attemptNav: AttemptNavigatorService,
              private answerColl: AnswerCollectionService) { }

  ngOnInit() {
    this.currentQuestion = this.attemptNav.CurrentQuestion.map(q => q.Question);
    this.currentQuestionId = this.attemptNav.CurrentQuestion.map(q => q.Id);
    this.currentQuestionChoices = this.attemptNav.CurrentQuestion.map(q => q.Choices.map(c => c.Choice));
    this.attemptNav.CurrentQuestion.subscribe(q => {
      this._currentQuestionIdForAnswer = q.Id;
      if (this.answerColl.FindAnswerIndexForQuestion(q.Id) !== null) {
        this.currentAnswer = this.answerColl.FindAnswerIndexForQuestion(q.Id);
      } else {
        this.currentAnswer = null;
      }
    });
    this.canMoveToNextQuestion = this.attemptNav.CanMoveToNextQuestion.do(v => console.log('can move to next: ' + v));
  }

  moveToNextQuestion() {
    console.log('recording answer for current question: ' + this.currentAnswer);
    this.answerColl.AddAnwer(this._currentQuestionIdForAnswer , this.currentAnswer);

    console.log('moving to next question using attempt navigator');
    this.attemptNav.MoveToNextQuestion();
  }

  moveToPreviousQuestion() {
    console.log('recording answer for current question: ' + this.currentAnswer);
    this.answerColl.AddAnwer(this._currentQuestionIdForAnswer , this.currentAnswer);

    console.log('moving to previous question using attempt navigator');
    this.attemptNav.MoveToPreviousQuestion();
  }

}
