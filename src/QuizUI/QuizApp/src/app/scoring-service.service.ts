import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/rx';
import 'rxjs/rx';

import { ConfigService } from './config-service.service';
import { AnswerCollection } from './answercollection';
import { Score } from './score';

@Injectable()
export class ScoringServiceService {
  private _scoringRequests: BehaviorSubject<ScoringRequest> = new BehaviorSubject(null);

  constructor(private http: Http, private config: ConfigService) { }

  ScoreQuiz(quizId: string, attemptId: string, answers: AnswerCollection) {
     this._scoringRequests.next( {'quizid': quizId, 'attemptid': attemptId, 'answers': answers} );
  }

  get Score(): Observable<Score> {
    return Observable.combineLatest(
       this.config.urlForScoring,
       this._scoringRequests.filter(e => (e !== null)))
           .flatMap(e => this.http.post(e[0](e[1].quizid, e[1].attemptid), e[1].answers))
           .map(this.mapToScore);
  }

   private mapToScore(resp: Response): Score {
    console.log('mapping..');
    let body = resp.json();
    let score = <Score> body;
    console.log('score body that came back: ' + score);
    return score;
   }
}

interface ScoringRequest {
  quizid: string;
  attemptid: string;
  answers: AnswerCollection;
}