/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ScoringServiceService } from './scoring-service.service';

describe('Service: ScoringService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ScoringServiceService]
    });
  });

  it('should ...', inject([ScoringServiceService], (service: ScoringServiceService) => {
    expect(service).toBeTruthy();
  }));
});
