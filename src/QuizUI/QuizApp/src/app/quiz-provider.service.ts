import { Injectable } from '@angular/core';
import 'rxjs/Rx';

import { Observable } from 'rxjs/Observable';
import { Http, Response } from '@angular/http';
import { Quiz } from './Quiz';

@Injectable()
export class QuizProviderService {

  private backendurl = 'http://demoquizapi.azurewebsites.net/api/Quizes';

  constructor(private http: Http) { }

  // OLD:
  GetAvailableQuizes(): Quiz[] {
    return [new Quiz(0, 'The great App Service Quiz'),
            new Quiz(1, 'Some other random Quiz')];
  }

  GetAvailableQuizesRx(): Observable<Quiz[]> {
    return this.http.get(this.backendurl)
                    .do(() => {console.log('observables in action'); })
                    .map(this.mapToQuiz)
                    .catch(this.handleError);
  }

  private mapToQuiz(resp: Response): Quiz[] {
    console.log('mapping..');
    let body = resp.json();
    let quizes = body.map(e => new Quiz(e.Id, e.Name) );
    return quizes;
  }

  private handleError(error: any) {
    return Observable.throw('failed fetching quizes');
  }
}
