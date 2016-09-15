/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { QuizProviderService } from './quiz-provider.service';

describe('Service: QuizProvider', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [QuizProviderService]
    });
  });

  it('should return quizes', inject([QuizProviderService], (service: QuizProviderService) => {
    expect(service.GetAvailableQuizes()[0].id).not.toBeNull;        
  }));
});
