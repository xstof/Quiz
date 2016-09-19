import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/rx';
import 'rxjs/rx';

import { ConfigService } from './config-service.service';
import { Attempt } from './attempt';

@Injectable()
export class AttemptProviderService {
   private _attemptStartRequests: BehaviorSubject<string> = new BehaviorSubject(null);

   constructor(private config: ConfigService, private http: Http) { }

   StartAttempt(email: string) {
     this._attemptStartRequests.next(email);
   }

   get CurrentAttempt(): Observable<Attempt> {
     return Observable.combineLatest(this.config.urlForQuizAttempts, this._attemptStartRequests)
               .flatMap(e => this.http.post(e[0], {'Email': e[1]}))
               .map(this.mapToAttempt);
   }

   private mapToAttempt(resp: Response): Attempt {
    console.log('mapping..');
    let body = resp.json();
    let attempt = <Attempt> body;
    return attempt;
   }
}
