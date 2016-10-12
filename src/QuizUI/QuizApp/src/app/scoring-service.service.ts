import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/rx';
import 'rxjs/rx';

import { ConfigService } from './config-service.service';
import { AnswerCollection } from './answercollection';
import { Score } from './score';

import { AuthenticatedHttpClient } from './authenticatedHttpClient';

@Injectable()
export class ScoringService {
  private _scoringRequests: BehaviorSubject<ScoringRequest> = new BehaviorSubject(null);

  constructor(private http: AuthenticatedHttpClient, private config: ConfigService) {
  }

  ScoreQuiz(quizId: string, attemptId: string, answers: AnswerCollection) {
     this._scoringRequests.next( {'quizid': quizId, 'attemptid': attemptId, 'answers': answers} );
  }

  get Score(): Observable<Score> {
    return Observable.combineLatest(
       this.config.urlForScoring,
       this._scoringRequests
           .publishReplay(1).refCount()
           .filter(e => (e !== null)))
           .flatMap(e => this.http.post(e[0](e[1].quizid, e[1].attemptid), e[1].answers))
           .map(this.mapToScore)
           .publishReplay(1)
           .refCount();
  }

   private mapToScore(resp: Response): Score {
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
