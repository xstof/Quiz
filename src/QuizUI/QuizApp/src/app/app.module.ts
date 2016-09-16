import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { QuizAppComponent } from './quiz-app/quiz-app.component';

@NgModule({
  declarations: [
    QuizAppComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule
  ],
  providers: [],
  bootstrap: [QuizAppComponent]
})
export class AppModule { }
