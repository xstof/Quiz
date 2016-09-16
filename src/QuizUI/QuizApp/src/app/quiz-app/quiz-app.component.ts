import { Component, OnInit } from '@angular/core';
import { QuizProviderService } from '../quiz-provider.service';
import { Observable } from 'rxjs/Observable';
import { Quiz } from '../quiz';
import 'rxjs/Rx';

@Component({
  selector: 'app-quiz-app',
  templateUrl: './quiz-app.component.html',
  styleUrls: ['./quiz-app.component.css'],
  providers: [QuizProviderService]
})
export class QuizAppComponent implements OnInit {
  private _quizes: Quiz[] = null;

  constructor(private quizProvider: QuizProviderService) { }

  ngOnInit() {
    this.getquizes();
  }

  get quizes(): Quiz[] {
    return this._quizes;
  }

  get quizesrx(): Observable<Quiz[]> {
    return this.quizProvider.GetAvailableQuizesRx();
  }

  getquizes() {
    this.quizProvider.GetAvailableQuizesRx().subscribe(
      q => this._quizes = q,
      e => console.log('error: ' + e)
    );
  }

}
