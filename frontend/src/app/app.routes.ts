import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    // This path segment matches the start of your desired URL
    path: 'jira/software/projects',
    // Lazy load the module responsible for handling project-related views
    loadChildren: () =>
      import('./features/project-managment/project-routing.module').then(
        (m) => m.ProjectRoutingModule
      ),
  },
];
