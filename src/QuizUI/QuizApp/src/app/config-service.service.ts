import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs/rx';

@Injectable()
export class ConfigService {
  private _baseServiceUrlSubject = new BehaviorSubject('http://demoquizapi.azurewebsites.net/api');
  private _urlForAvailableQuizesSubject = new BehaviorSubject('http://demoquizapi.azurewebsites.net/api/Quizes');

  constructor() {
    console.log('created new config service');
  }

  get baseUrl(): Observable<string> {
    return this._baseServiceUrlSubject;
  }

  setBaseUrl(url: string) {
    this._baseServiceUrlSubject.next(url);
    console.log('updated base url: ' + url);
  }

  get urlForAvailableQuizes(): Observable<string> {
    return this._baseServiceUrlSubject.map(u => u + '/Quizes');
  }

  // TODO: remove
  setUrlForAvailableQuizes(url: string) {
    this._urlForAvailableQuizesSubject.next(url);
    console.log('updated url: ' + url);
  }
}
