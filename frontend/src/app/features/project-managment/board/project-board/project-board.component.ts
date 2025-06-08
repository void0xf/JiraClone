import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CdkDragDrop, DragDropModule, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { Issue, IssueStatus } from '../../../../models/issue.model'; 
import { IssueService } from '../../../../core/services/issue.service';
import { Store } from '@ngrx/store';
import { ProjectState } from '../../store/project.state';
import { selectSelectedProject } from '../../store/project.selectors';
import { Project } from '../../../../core/models/project.model';
import { ProjectBoardColumnComponent } from '../project-board-column/project-board-column.component';
import { Subject } from 'rxjs';
import { filter, switchMap, takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-project-board',
  imports: [CommonModule, ProjectBoardColumnComponent, DragDropModule],
  templateUrl: './project-board.component.html',
  styleUrls: ['./project-board.component.scss'],
})
export class ProjectBoardComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  columns: { title: IssueStatus; items: Issue[] }[] = [
    { title: IssueStatus.ToDo, items: [] },
    { title: IssueStatus.InProgress, items: [] },
    { title: IssueStatus.Done, items: [] },
  ];

  constructor(
    private issueService: IssueService,
    private store: Store<ProjectState>
  ) {}

  ngOnInit(): void {
    this.store
      .select(selectSelectedProject)
      .pipe(
        filter((project): project is Project => !!project),
        switchMap((project) =>
          this.issueService.getIssuesByProjectId(project.id)
        ),
        takeUntil(this.destroy$)
      )
      .subscribe((issues) => {
        if (issues) {
          this.columns = [
            {
              title: IssueStatus.ToDo,
              items: issues.filter((issue) => issue.status === IssueStatus.ToDo),
            },
            {
              title: IssueStatus.InProgress,
              items: issues.filter(
                (issue) => issue.status === IssueStatus.InProgress
              ),
            },
            {
              title: IssueStatus.Done,
              items: issues.filter((issue) => issue.status === IssueStatus.Done),
            },
          ];
        }
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  drop(event: CdkDragDrop<Issue[]>) {
    const draggedItem = event.item.data as Issue;
    const sourceList = event.previousContainer.data;
    const actualPreviousIndex = sourceList.findIndex(
      (i) => i.id === draggedItem.id
    );

    if (event.previousContainer === event.container) {
      moveItemInArray(
        event.container.data,
        actualPreviousIndex,
        event.currentIndex
      );
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        actualPreviousIndex,
        event.currentIndex
      );

      this.issueService
        .updateIssue(draggedItem.id, {
          status: event.container.id as IssueStatus,
        })
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (updatedIssue) => {
          },
          error: (err) => {
            transferArrayItem(
              event.container.data,
              event.previousContainer.data,
              event.currentIndex,
              actualPreviousIndex
            );
          },
        });
    }
  }
}
