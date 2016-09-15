import { Injectable } from '@angular/core';
import { Quiz } from './Quiz';

@Injectable()
export class QuizProviderService {

  constructor() { }

  GetAvailableQuizes(): Quiz[] {
    return [new Quiz(0, 'The great App Service Quiz'),
            new Quiz(1, 'Some other random Quiz')];
  }
}
