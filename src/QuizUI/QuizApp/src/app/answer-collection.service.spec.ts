/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { AnswerCollectionService } from './answer-collection.service';

describe('Service: AnswerCollection', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AnswerCollectionService]
    });
  });

  it('should ...', inject([AnswerCollectionService], (service: AnswerCollectionService) => {
    expect(service).toBeTruthy();
  }));
});
