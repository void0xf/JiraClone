// src/app/features/project/project-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProjectLayoutComponent } from './project-layout.component';
import { ProjectSummaryComponent } from '../../../features/project-managment/project-summary/project-summary.component';

const routes: Routes = [
  {
    path: ':project_key',
    component: ProjectLayoutComponent,
    children: [
      { path: '', redirectTo: 'summary', pathMatch: 'full' },

      {
        path: 'summary', // Matches the 'category' segment in your example URL
        component: ProjectSummaryComponent,
        title: 'Project Summary', // Optional: Set browser title
      },
      {
        path: 'timeline', // Matches the 'category' segment in your example URL
        component: ProjectSummaryComponent,
        title: 'Project Timeline',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProjectRoutingModule {}
