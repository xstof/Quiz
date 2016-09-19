import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { AttemptNavigatorService } from '../attempt-navigator.service';
import { Question } from '../attempt';

@Component({
  selector: 'app-attempt',
  templateUrl: './attempt.component.html',
  styleUrls: ['./attempt.component.css']
})
export class AttemptComponent implements OnInit {

  constructor(private attemptNav: AttemptNavigatorService) { }

  ngOnInit() {
  }

  get currentQuestion(): Observable<Question> {
    return this.attemptNav.CurrentQuestion;
  }

  moveToNextQuestion() {
    this.attemptNav.MoveToNextQuestion();
  }

  moveToPreviousQuestion() {
    this.attemptNav.MoveToPreviousQuestion();
  }

}
