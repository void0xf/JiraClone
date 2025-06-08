import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, filter, map, switchMap } from 'rxjs';
import { Project } from '../../../core/models/project.model';
import { ProjectState } from '../store/project.state';
import * as ProjectActions from '../store/actions/project.actions';
import * as ProjectSelectors from '../store/project.selectors';
import { AsyncPipe, NgIf } from '@angular/common';

@Component({
  selector: 'app-project-summary',
  standalone: true,
  imports: [AsyncPipe, NgIf],
  templateUrl: './project-summary.component.html',
  styleUrl: './project-summary.component.scss',
})
export class ProjectSummaryComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private store = inject(Store<ProjectState>);

  project$: Observable<Project | null>;
  isLoading$: Observable<boolean>;
  error$: Observable<any | null>;

  constructor() {
    this.project$ = this.store.select(
      ProjectSelectors.selectCurrentDetailedProject
    );
    this.isLoading$ = this.store.select(
      ProjectSelectors.selectCurrentProjectLoading
    );
    this.error$ = this.store.select(ProjectSelectors.selectCurrentProjectError);
  }

  ngOnInit(): void {
    this.route.paramMap
      .pipe(
        map((params) => params.get('project_key')),
        filter((projectKey) => !!projectKey)
      )
      .subscribe((projectKey) => {
        if (projectKey) {
          this.store.dispatch(ProjectActions.loadSingleProject({ projectKey }));
        }
      });
  }
}
