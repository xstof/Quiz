/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { AttemptProviderService } from './attempt-provider.service';

describe('Service: AttemptProvider', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AttemptProviderService]
    });
  });

  it('should ...', inject([AttemptProviderService], (service: AttemptProviderService) => {
    expect(service).toBeTruthy();
  }));
});
