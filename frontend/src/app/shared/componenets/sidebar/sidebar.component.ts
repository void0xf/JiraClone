import { Component, inject, effect, OnInit, OnDestroy } from '@angular/core';
import { AsyncPipe, NgIf } from '@angular/common';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { HlmIconDirective } from '@spartan-ng/ui-icon-helm';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {
  lucideCircleUser,
  lucideClock,
  lucideStar,
  lucideLayoutGrid,
  lucideLayoutDashboard,
  lucideFolder,
  lucideUsers,
  lucideFilter,
  lucidePlus,
  lucideMoveVertical,
  lucideChevronRight,
  lucideGrid2x2Plus,
  lucideLayers,
  lucideMegaphone,
  lucideSettings2,
} from '@ng-icons/lucide';
import { SidebarService } from '../../../core/services/sidebar.service';
import {
  SidebarItemType,
  SidebarSelectionService,
} from '../../../core/services/sidebar-selection.service';
import { ListTreeComponent } from '../list-tree/list-tree.component';
import { ListTreeItemComponent } from '../list-tree-item/list-tree-item.component';
import { LabelComponent } from '../label/label.component';
import { map, Observable, Subject, filter, switchMap, takeUntil } from 'rxjs';
import { Project } from '../../../core/models/project.model';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import * as ProjectSelectors from '../../../features/project-managment/store/project.selectors';
import * as ProjectActions from '../../../features/project-managment/store/actions/project.actions';
import { ProjectState } from '../../../features/project-managment/store/project.state';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    NgIf,
    AsyncPipe,
    HlmButtonDirective,
    HlmIconDirective,
    NgIcon,
    ListTreeComponent,
    ListTreeItemComponent,
    LabelComponent,
  ],
  providers: [
    provideIcons({
      lucideCircleUser,
      lucideClock,
      lucideStar,
      lucideLayoutGrid,
      lucideLayoutDashboard,
      lucideFolder,
      lucideUsers,
      lucideFilter,
      lucidePlus,
      lucideMoveVertical,
      lucideChevronRight,
      lucideGrid2x2Plus,
      lucideLayers,
      lucideMegaphone,
      lucideSettings2,
    }),
  ],
  templateUrl: './sidebar.component.html',
  styles: [
    `
      :host {
        display: block;
      }
    `,
  ],
})
export class SidebarComponent implements OnInit, OnDestroy {
  SidebarItemType = SidebarItemType;
  activeItem: string | null = 'temple-run';

  projectsData$: Observable<Project[]>;

  projectsIsLoading$: Observable<boolean>;

  private destroy$ = new Subject<void>();

  // private store = inject(Store<ProjectState>);

  constructor(
    private sidebarService: SidebarService,
    private selectionService: SidebarSelectionService,
    private router: Router,
    private route: ActivatedRoute,
    private store: Store<ProjectState>
  ) {
    this.projectsData$ = this.store.select(ProjectSelectors.selectAllProjects);
    this.projectsIsLoading$ = this.store.select(
      ProjectSelectors.selectProjectsLoading
    );
  }

  ngOnInit(): void {
    this.store.dispatch(ProjectActions.loadProjects());

    this.route.paramMap
      .pipe(
        map((params) => params.get('project_key')),
        filter((projectKey): projectKey is string => !!projectKey),
        switchMap((projectKey) =>
          this.store.select(
            ProjectSelectors.selectProjectByProjectKey,
            projectKey
          )
        ),
        filter((project): project is Project => !!project),
        takeUntil(this.destroy$)
      )
      .subscribe((project) => {
        this.store.dispatch(
          ProjectActions.selectProject({ projectId: project.id })
        );
        this.selectionService.selectItem(project.id, SidebarItemType.PROJECT);
        this.activeItem = project.name;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  get isOpen() {
    return this.sidebarService.isOpen;
  }

  selectMenuItem(id: string, type: SidebarItemType): void {
    if (id == 'for-you') {
      this.router.navigate(['jira/your-work']);
    }
    this.selectionService.selectItem(id, type);
  }

  isMenuItemSelected(id: string): boolean {
    return this.selectionService.isItemSelected(id, SidebarItemType.MENU_ITEM);
  }

  handleItemClick(itemName: string): void {
    this.activeItem = itemName;
  }
  handleProjectItemClick(project: Project): void {
    this.activeItem = project.name;
    this.store.dispatch(ProjectActions.selectProject({ projectId: project.id }));
    this.router.navigate([
      '/jira/software/projects',
      project.projectKey,
      'summary',
    ]);
  }
  handleProjectsAdd(event: MouseEvent): void {
    console.log('Add Project clicked');
  }

  handleProjectsMore(event: MouseEvent): void {
    console.log('More Project actions');
  }

  handleItemAdd(itemName: string, event: MouseEvent): void {
    console.log(`Add action on item: ${itemName}`);
  }

  handleAppsAdd(event: MouseEvent): void {
    console.log('Add App clicked');
  }

  handleAppsMore(event: MouseEvent): void {
    console.log('More App actions');
  }

  handlePlansAdd(event: MouseEvent): void {
    console.log('Add Plan clicked');
  }

  handlePlansMore(event: MouseEvent): void {
    console.log('More Plan actions');
  }
}
