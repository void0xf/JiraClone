import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { AsyncPipe, NgIf } from '@angular/common';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideEllipsis } from '@ng-icons/lucide';
import { HlmIconDirective } from '@spartan-ng/ui-icon-helm';
import { Store } from '@ngrx/store';
import { ActivatedRoute } from '@angular/router';
import { Subscription, Observable } from 'rxjs';
import { map, filter, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { ProjectState } from '../store/project.state';
import * as ProjectSelectors from '../store/project.selectors';
import { Project } from '../../../core/models/project.model';

@Component({
  selector: 'app-project-header',
  standalone: true,
  imports: [HlmIconDirective, NgIcon, AsyncPipe, NgIf],
  providers: [
    provideIcons({
      lucideEllipsis,
    }),
  ],
  templateUrl: './project-header.component.html',
  styleUrl: './project-header.component.scss',
})
export class ProjectHeaderComponent implements OnInit {
  selectedProject$: Observable<Project | null>;
  projectName: string = 'Loading Project...'; // Fallback or initial state
  private store = inject(Store<ProjectState>);

  constructor(private route: ActivatedRoute) {
    this.selectedProject$ = this.store.select(
      ProjectSelectors.selectSelectedProject
    );
  }

  ngOnInit(): void {
    this.selectedProject$.subscribe((project) => {
      if (project) {
        this.projectName = project.name;
      } else {
        // Optionally, handle the case where no project is selected or project is null
        // For example, try to load based on route param or set a default
        this.projectName = 'No Project Selected';
      }
    });
  }
}
