/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { AttemptNavigatorService } from './attempt-navigator.service';

describe('Service: AttemptNavigator', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AttemptNavigatorService]
    });
  });

  it('should ...', inject([AttemptNavigatorService], (service: AttemptNavigatorService) => {
    expect(service).toBeTruthy();
  }));
});
