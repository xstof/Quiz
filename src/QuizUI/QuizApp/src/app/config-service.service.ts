import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable, BehaviorSubject } from 'rxjs/rx';

@Injectable()
export class ConfigService {
  // private _baseServiceUrlSubject = new BehaviorSubject(window.location.host + '/api');
  // private _baseServiceUrlSubject = new BehaviorSubject('http://demoquizapi.azurewebsites.net/api');
  private _baseServiceUrlSubject = null;

  constructor(http: Http) {
    console.log('created new config service');

    this._baseServiceUrlSubject = http.get('/apibaseurl')
                                      .do(u => console.log('using api base url: ' + u))
                                      .map(u => u.json().apibaseurl);
  }

  get baseUrl(): Observable<string> {
    return this._baseServiceUrlSubject.distinct().share();
  }

  setBaseUrl(url: string) {
    this._baseServiceUrlSubject.next(url);
    console.log('updated base url: ' + url);
  }

  get urlForAvailableQuizes(): Observable<string> {
    return this._baseServiceUrlSubject.distinct().map(u => u + '/quizzes');
  }

  get urlForQuizAttempts(): Observable<(quizid: string) => string> {
      return this._baseServiceUrlSubject.distinct().map(u =>
        function(quizid: string) {
          return u + `/quizzes/${quizid}/attempts`;
        } );
  }

  get urlForScoring(): Observable<(quizid: string, attemptid: string) => string> {
    return this._baseServiceUrlSubject.distinct().map(u =>
      function(quizid: string, attemptid: string) {
        return u + `/quizzes/${quizid}/attempts/${attemptid}/score`;
      }
    );
  }
}
