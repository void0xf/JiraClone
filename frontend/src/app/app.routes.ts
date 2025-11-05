import { Routes } from '@angular/router';
import { ForYouComponent } from './features/for-you/for-you.component';
import { SignInComponent } from './features/auth/sign-in/sign-in.component';
import { SignUpComponent } from './features/auth/sign-up/sign-up.component';
import { authGuard } from './core/guards/auth.guard';
import { guestGuard } from './core/guards/guest.guard';  // ← Import this!

export const routes: Routes = [
  {
    path: 'jira/software/projects',
    loadChildren: () =>
      import('./core/layout/project-layout/project.routes').then(
        (m) => m.PROJECT_ROUTES
      ),
    canActivate: [authGuard]
  },
  {
    path: 'jira/your-work',
    component: ForYouComponent,
    canActivate: [authGuard]
  },
  {
    path: 'jira/software/for-you',
    component: ForYouComponent,
    canActivate: [authGuard]
  },

  {
    path: 'jira/software/sign-in',
    component: SignInComponent,
    canActivate: [guestGuard]  
  },
  {
    path: 'jira/software/sign-up',
    component: SignUpComponent,
    canActivate: [guestGuard]  
  },
];
