import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QuizAppComponent } from './quiz-app/quiz-app.component';
import { SettingsComponent } from './settings/settings.component';
import { AttemptComponent } from './attempt/attempt.component';

const appRoutes: Routes = [
  { path: 'settings', component: SettingsComponent },
  { path: 'attempt', component: AttemptComponent },
  { path: '', component: QuizAppComponent }
//{ path: '**', component: PageNotFoundComponent }
];

export const appRoutingProviders: any[] = [

];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
