import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { AttemptNavigatorService } from '../attempt-navigator.service';
import { Question, Choice } from '../attempt';

@Component({
  selector: 'app-attempt',
  templateUrl: './attempt.component.html',
  styleUrls: ['./attempt.component.css']
})
export class AttemptComponent implements OnInit {

  currentQuestion: Observable<string> = null;
  currentQuestionId: Observable<string> = null;
  currentQuestionChoices: Observable<string[]> = null;
  currentAnswer: number;
  answers: number[];

  constructor(private attemptNav: AttemptNavigatorService) { }

  ngOnInit() {
    this.currentQuestion = this.attemptNav.CurrentQuestion.map(q => q.Question);
    this.currentQuestionId = this.attemptNav.CurrentQuestion.map(q => q.Id);
    this.currentQuestionChoices = this.attemptNav.CurrentQuestion.map(q => q.Choices.map(c => c.Choice));
  }

  moveToNextQuestion() {
    console.log('recording answer for current question: ' + this.currentAnswer);
    // TODO
    console.log('moving to next question using attempt navigator');
    this.attemptNav.MoveToNextQuestion();
  }

  moveToPreviousQuestion() {
    this.attemptNav.MoveToPreviousQuestion();
  }

}
