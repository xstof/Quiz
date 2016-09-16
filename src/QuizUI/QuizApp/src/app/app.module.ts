import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { AppComponent } from './app.component';

import { QuizAppComponent } from './quiz-app/quiz-app.component';

@NgModule({
  declarations: [
    AppComponent,
    QuizAppComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule
  ],
  providers: [],
  bootstrap: [AppComponent]
  // bootstrap: [QuizAppComponent]
})
export class AppModule { }
