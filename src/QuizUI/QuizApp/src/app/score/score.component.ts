import { Component, OnInit } from '@angular/core';

import { ScoringService } from '../scoring-service.service';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/rx';
import { Score } from '../score';

@Component({
  selector: 'app-score',
  templateUrl: './score.component.html',
  styleUrls: ['./score.component.css']
})
export class ScoreComponent implements OnInit {
  score: number = null;

  constructor(private scoringsvc: ScoringService) {
    this.scoringsvc.Score.subscribe(s => {
      this.score = s.ScoreInPercentage;
    });
  }

  ngOnInit() {

  }

}
