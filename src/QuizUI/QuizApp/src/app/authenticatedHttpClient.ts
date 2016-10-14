import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {ConfigService} from './config-service.service';

@Injectable()
export class AuthenticatedHttpClient {

  private _apimKey: string = '';

  constructor(private http: Http, private config: ConfigService) {
      this.config.apimKey.subscribe(k => {
        console.log('got new apim key: ' + k);
        this._apimKey = k;
      });
  }


  createApimHeader(headers: Headers) {
    headers.append('Ocp-Apim-Subscription-Key', this._apimKey);
    headers.append('Ocp-Apim-Trace', 'true');
  }

  get(url) {
    let headers = new Headers();
    this.createApimHeader(headers);
    return this.http.get(url, {
      headers: headers
    });
  }

  post(url, data) {
    let headers = new Headers();
    this.createApimHeader(headers);
    return this.http.post(url, data, {
      headers: headers
    });
  }
}