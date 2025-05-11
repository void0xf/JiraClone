// src/app/features/project/project-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProjectLayoutComponent } from '../../core/layout/project-layout/project-layout.component';
import { ProjectSummaryComponent } from './project-summary/project-summary.component';

// --- Import Components ---

// Routes defined here are relative to '/jira/software/projects'
const routes: Routes = [
  {
    // This path segment captures the dynamic project key (e.g., 'PROJ-123')
    path: ':project_key',
    component: ProjectLayoutComponent, // Use ProjectLayoutComponent as the container/shell for this project
    children: [
      // Default view within a project (if no category specified) - redirect to summary
      { path: '', redirectTo: 'summary', pathMatch: 'full' },

      // --- Define routes for each specific category ---
      // Matches: jira/software/projects/{project_key}/summary
      {
        path: 'summary', // Matches the 'category' segment in your example URL
        component: ProjectSummaryComponent,
        title: 'Project Summary', // Optional: Set browser title
      },
      // Matches: jira/software/projects/{project_key}/board

      // Matches: jira/software/projects/{project_key}/settings
      // {
      //    path: 'settings',
      //    component: SettingsPageComponent,
      //    title: 'Project Settings'
      // },

      // ... Add routes for all other valid project categories ('reports', 'issues', etc.)

      // Optional: Handle unknown categories within a known project
      // { path: '**', redirectTo: 'summary' } // Or show a specific 'CategoryNotFoundComponent'
    ],
  },

  // Optional: Route for a page listing all projects (if needed)
  // { path: '', component: ProjectListPageComponent } // This would match '/jira/software/projects' exactly
];

@NgModule({
  // Use forChild() in feature routing modules
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProjectRoutingModule {}
