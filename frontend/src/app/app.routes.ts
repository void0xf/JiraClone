import { Routes } from '@angular/router';
import { SignInComponent } from './features/auth/sign-in/sign-in.component';

export const routes: Routes = [
  {
    // This path segment matches the start of your desired URL
    path: 'jira/software/projects',
    // Lazy load the module responsible for handling project-related views
    loadChildren: () =>
      import('./core/layout/project-layout/project-routing.module').then(
        (m) => m.ProjectRoutingModule
      ),
  },
  {
    path: 'jira/software/sign-in',
    component: SignInComponent,
  },
];
