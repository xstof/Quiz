import { Injectable } from '@angular/core';

import { Answer, AnswerCollection } from './answercollection';

@Injectable()
export class AnswerCollectionService {
  private _answercoll: AnswerCollection = null;

  constructor() {
    this._answercoll = new AnswerCollection();
  }

  AddAnwer(questionId: string, answerIndex: number) {
    let newAnswer = this._findAnswerForQuestion(questionId);
    if (newAnswer === null || newAnswer === undefined) {
      newAnswer = new Answer(questionId, answerIndex);
      this._answercoll.Answers.push(newAnswer);
    } else { newAnswer.Answer = answerIndex; }

  }

  private _findAnswerForQuestion(questionId: string): Answer {
    return this._answercoll.Answers.find(a => a.QuestionId === questionId);
  }

  FindAnswerIndexForQuestion(questionId: string): number {
    let answer = this._answercoll.Answers.find(a => a.QuestionId === questionId);
    if (answer !== null && answer !== undefined) {
      return answer.Answer;
    } else {
      return null;
    }
  }

}
