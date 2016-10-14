import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/rx';
import 'rxjs/rx';

import { ConfigService } from './config-service.service';
import { Attempt } from './attempt';
import { AuthenticatedHttpClient } from './authenticatedHttpClient';

@Injectable()
export class AttemptProviderService {
   private _attemptStartRequests: BehaviorSubject<attemptRequest> = new BehaviorSubject(null);

   constructor(private config: ConfigService, private http: AuthenticatedHttpClient) { }

   StartAttempt(email: string, quizid: string) {
     this._attemptStartRequests.next( {'email': email, 'quizid': quizid} );
   }

   get CurrentAttempt(): Observable<Attempt> {
     return Observable.combineLatest(
       this.config.urlForQuizAttempts,
       this._attemptStartRequests.filter(e => (e !== null)))
               .flatMap(e => this.http.post(e[0](e[1].quizid), {'Email': e[1].email}))
               .do(() => console.log('created new attempt'))
               .map(this.mapToAttempt);
   }

   private mapToAttempt(resp: Response): Attempt {
    console.log('mapping..');
    let body = resp.json();
    let attempt = <Attempt> body;
    console.log('attempt body that came back: ' + attempt);
    return attempt;
   }
}

interface attemptRequest {
  email: string;
  quizid: string;
}
