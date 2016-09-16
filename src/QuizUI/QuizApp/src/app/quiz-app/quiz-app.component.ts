import { Component, OnInit } from '@angular/core';
import { QuizProviderService } from '../quiz-provider.service';
import { Observable } from 'rxjs/Observable';
import { Quiz } from '../quiz';

@Component({
  selector: 'app-quiz-app',
  templateUrl: './quiz-app.component.html',
  styleUrls: ['./quiz-app.component.css']
})
export class QuizAppComponent implements OnInit {
  private _quizes: Quiz[] = null;

  constructor(private quizProvider: QuizProviderService) { }

  ngOnInit() {}

  get quizes(): Quiz[] {
    return this._quizes;
  }

  get quizesrx(): Observable<Quiz[]> {
    return this.quizProvider.AvailableQuizes;
  }
}
