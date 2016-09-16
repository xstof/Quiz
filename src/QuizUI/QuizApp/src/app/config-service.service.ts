import { Injectable } from '@angular/core';
// import { Observable } from 'rxjs/Observable';

@Injectable()
export class ConfigService {
  private _backendUrlForAvailableQuizes = 'http://demoquizapi.azurewebsites.net/api/Quizes';

  constructor() { }

  get backendUrlForAvailableQuizes(): string {
    return this._backendUrlForAvailableQuizes;
  }

  set backendUrlForAvailableQuizes(url: string) {
    this._backendUrlForAvailableQuizes = url;
  }
}
