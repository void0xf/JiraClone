import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, map, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';
import * as ProjectActions from './actions/project.actions';
import { ProjectService } from '../../../core/services/project.service';

@Injectable()
export class ProjectEffects {
  private actions$ = inject(Actions);
  private projectService = inject(ProjectService);

  loadProjects$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProjectActions.loadProjects),
      switchMap(() =>
        this.projectService.getProjects().pipe(
          map((projects) => ProjectActions.loadProjectsSuccess({ projects })),
          catchError((error) =>
            of(ProjectActions.loadProjectsFailure({ error }))
          )
        )
      )
    )
  );

  loadSingleProject$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProjectActions.loadSingleProject),
      switchMap((action) =>
        this.projectService.getProjectByKey(action.projectKey).pipe(
          map((project) =>
            ProjectActions.loadSingleProjectSuccess({ project })
          ),
          catchError((error) =>
            of(ProjectActions.loadSingleProjectFailure({ error }))
          )
        )
      )
    )
  );
}
