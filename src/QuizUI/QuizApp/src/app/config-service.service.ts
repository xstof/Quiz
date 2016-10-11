import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject, Subject, ReplaySubject } from 'rxjs/rx';
import 'rxjs/rx';
import { Http } from '@angular/http';

@Injectable()
export class ConfigService {
  // private _baseServiceUrlSubject = new BehaviorSubject(window.location.host + '/api');
  // private _baseServiceUrlSubject = new BehaviorSubject('http://demoquizapi.azurewebsites.net/api');
  private _baseServiceUrlObservable: Observable<string> = null;
  private _manualBaseServiceUrlOverrideSubject = new ReplaySubject<string>(1);
  private _compoundBaseUrlObservable: Observable<string> = null;
  private _apimKeySubject = new BehaviorSubject('');

  constructor(private http: Http) {
    console.log('created new config service');

    this._baseServiceUrlObservable = http.get('/apibaseurl')
                                      .do(u => console.log('using api base url: ' + u))
                                      .map(u => u.json().apibaseurl);
    this._compoundBaseUrlObservable = this._baseServiceUrlObservable
                                          .concat(this._manualBaseServiceUrlOverrideSubject);
  }

  get baseUrl(): Observable<string> {
    return this._compoundBaseUrlObservable.distinct().share();
  }

  setBaseUrl(url: string) {
    this._manualBaseServiceUrlOverrideSubject.next(url);
    console.log('updated base url: ' + url);
  }

  get urlForAvailableQuizes(): Observable<string> {
    return this._compoundBaseUrlObservable.distinct().map(u => u + '/quizzes');
  }

  get urlForQuizAttempts(): Observable<(quizid: string) => string> {
      return this._compoundBaseUrlObservable.distinct().map(u =>
        function(quizid: string) {
          return u + `/quizzes/${quizid}/attempts`;
        } );
  }

  get urlForScoring(): Observable<(quizid: string, attemptid: string) => string> {
    return this._compoundBaseUrlObservable.distinct().map(u =>
      function(quizid: string, attemptid: string) {
        return u + `/quizzes/${quizid}/attempts/${attemptid}/score`;
      }
    );
  }

  setAPIMKey(key: string) {
    this._apimKeySubject.next(key);
  }

  get apimKey(): Observable<string> {
    return this._apimKeySubject;
  }
}
