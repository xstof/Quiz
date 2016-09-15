import { Component, OnInit } from '@angular/core';
import { QuizProviderService } from '../quiz-provider.service';
import { Quiz } from '../quiz';

@Component({
  selector: 'app-quiz-app',
  templateUrl: './quiz-app.component.html',
  styleUrls: ['./quiz-app.component.css'],
  providers: [QuizProviderService]
})
export class QuizAppComponent implements OnInit {

  constructor(private quizProvider: QuizProviderService) { }

  ngOnInit() {
  }

  get quizes(): Quiz[] {
    return this.quizProvider.GetAvailableQuizes();
  }

}
