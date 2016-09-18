import { Injectable } from '@angular/core';
//import 'rxjs/Rx';

import { Observable } from 'rxjs/Observable';
import { Http, Response } from '@angular/http';
import { Quiz } from './quiz';
import { ConfigService } from './config-service.service';

@Injectable()
export class QuizProviderService {

  private _availableQuizes: Observable<Quiz[]> = null;

  constructor(private config: ConfigService, private http: Http) {
    this._availableQuizes = this.getAvailableQuizesRx();
  }
  private getAvailableQuizesRx(): Observable<Quiz[]> {
       return this.config.urlForAvailableQuizes
               .do(url => console.log('new backend url for fetching quizes: ' + url))
               .flatMap(url => this.http.get(url))
               .map(this.mapToQuiz)
               .do(quizes => console.log('fetched new set of quizes'))
               .catch(this.handleError);
  }

  get AvailableQuizes(): Observable<Quiz[]> {
    return this._availableQuizes;
  }
  private mapToQuiz(resp: Response): Quiz[] {
    console.log('mapping..');
    let body = resp.json();
    let quizes = body.map(e => new Quiz(e.Id, e.Name) );
    return quizes;
  }

  private handleError(error: any) {
    return Observable.throw('failed fetching quizes - error: ' + error);
  }
}
