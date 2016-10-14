import { Component, OnInit, OnDestroy } from '@angular/core';

import { ScoringService } from '../scoring-service.service';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject, Subscription } from 'rxjs/rx';
import { Score } from '../score';

@Component({
  selector: 'app-score',
  templateUrl: './score.component.html',
  styleUrls: ['./score.component.css']
})
export class ScoreComponent implements OnInit, OnDestroy {
  private _scoreSubscription: Subscription = null;
  score: number = null;

  constructor(private scoringsvc: ScoringService) {

  }

  ngOnInit() {
    this._scoreSubscription = this.scoringsvc.Score.subscribe(s => {
      this.score = s.ScoreInPercentage;
    });
  }

  ngOnDestroy() {
    this._scoreSubscription.unsubscribe();
  }

}
