import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs/rx';

@Injectable()
export class ConfigService {
  private _urlForAvailableQuizesSubject = new BehaviorSubject('http://demoquizapi.azurewebsites.net/api/Quizes');

  constructor() { console.log('created new config service'); }

  get urlForAvailableQuizes(): Observable<string> {
    return this._urlForAvailableQuizesSubject;
  }

  setUrlForAvailableQuizes(url: string) {
    this._urlForAvailableQuizesSubject.next(url);
    console.log('updated url: ' + url);
  }
}
