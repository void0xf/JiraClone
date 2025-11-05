import { Routes } from '@angular/router';
import { provideState } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { ProjectLayoutComponent } from './project-layout.component';
import { ProjectSummaryComponent } from '../../../features/project-managment/project-summary/project-summary.component';
import {
  projectFeatureKey,
  projectReducer,
} from '../../../features/project-managment/store/project.reducer';
import { ProjectEffects } from '../../../features/project-managment/store/project.effects';
import { ProjectBoardComponent } from '../../../features/project-managment/board/project-board/project-board.component';

export const PROJECT_ROUTES: Routes = [
  {
    path: ':project_key',
    component: ProjectLayoutComponent,
    providers: [
      provideState(projectFeatureKey, projectReducer),
      provideEffects([ProjectEffects]),
    ],
    children: [
      { path: '', redirectTo: 'summary', pathMatch: 'full' },
      {
        path: 'summary',
        component: ProjectSummaryComponent,
        title: 'Project Summary',
      },
      {
        path: 'timeline',
        component: ProjectSummaryComponent,
        title: 'Project Timeline',
      },
      {
        path: 'board',
        component: ProjectBoardComponent,
        title: 'Project Board',
      },
    ],
  },
];
