import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QuizAppComponent } from './quiz-app/quiz-app.component';
import { SettingsComponent } from './settings/settings.component';
import { AttemptComponent } from './attempt/attempt.component';
import { ScoreComponent } from './score/score.component';

const appRoutes: Routes = [
  { path: 'settings', component: SettingsComponent },
  { path: 'attempt', component: AttemptComponent },
  { path: 'score', component: ScoreComponent },
  { path: '', component: QuizAppComponent }
//{ path: '**', component: PageNotFoundComponent }
];

export const appRoutingProviders: any[] = [

];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
