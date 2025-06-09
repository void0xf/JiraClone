import { Component, inject, OnInit } from '@angular/core';
import { DialogRef, DIALOG_DATA, DialogModule } from '@angular/cdk/dialog';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DropdownComponent, DropdownOption, DropdownGroup } from '../../componenets/dropdown/dropdown.component';
import { Store } from '@ngrx/store';
import { ProjectState } from '../../../features/project-managment/store/project.state';
import * as ProjectActions from '../../../features/project-managment/store/actions/project.actions';
import { selectAllProjects, selectAllProjectsLoaded } from '../../../features/project-managment/store/project.selectors';
import { map, take, filter } from 'rxjs/operators';
import { IssueService } from '../../../core/services/issue.service';
import { Issue, IssueStatus, IssueType } from '../../../models/issue.model';
import { Project } from '../../../core/models/project.model';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-create-issue-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DialogModule, DropdownComponent],
  templateUrl: './create-issue-dialog.component.html',
  styleUrl: './create-issue-dialog.component.scss'
})
export class CreateIssueDialogComponent implements OnInit {
  dialogRef = inject<DialogRef<Issue>>(DialogRef<Issue>);
  data = inject(DIALOG_DATA, { optional: true });
  private store = inject<Store<ProjectState>>(Store);
  private issueService = inject(IssueService);
  private fb = inject(FormBuilder);

  createIssueForm: FormGroup;
  projectGroups$: Observable<DropdownGroup[]> = of([]);

  workTypes: DropdownOption[] = [
    { value: IssueType.Task, label: 'Task'  },
    { value: IssueType.Bug, label: 'Bug'  },
    { value: IssueType.Story, label: 'Story'  }
  ];

  statusTypes: DropdownOption[] = [
    { value: IssueStatus.ToDo, label: 'To Do' },
    { value: IssueStatus.InProgress, label: 'In Progress' },
    { value: IssueStatus.Done, label: 'Done' },
    { value: IssueStatus.Blocked, label: 'Blocked' },
    { value: IssueStatus.InReview, label: 'In Review' }
  ];

  constructor() {
    this.createIssueForm = this.fb.group({
      projectId: ['', Validators.required],
      issueType: [IssueType.Task, Validators.required],
      status: [IssueStatus.ToDo, Validators.required],
      summary: ['', Validators.required],
      description: [''],
      reporterId: [''], // This should be set to the current user's ID
      createAnother: [false]
    });
  }

  ngOnInit(): void {
    this.store.select(selectAllProjectsLoaded).pipe(
      take(1),
      filter(loaded => !loaded)
    ).subscribe(() => {
      this.store.dispatch(ProjectActions.loadProjects());
    });

    this.projectGroups$ = this.store.select(selectAllProjects).pipe(
      map(projects => this.mapProjectsToDropdownGroups(projects))
    );
  }

  private mapProjectsToDropdownGroups(projects: Project[]): DropdownGroup[] {
    const projectOptions: DropdownOption[] = projects.map(project => ({
      value: project.id,
      label: `${project.name} (${project.projectKey})`,
    }));

    return [
      {
        label: 'Recent Projects',
        options: projectOptions
      },
      {
        label: 'All Projects',
        options: projectOptions
      }
    ];
  }

  close(): void {
    this.dialogRef.close();
  }

  create(): void {
    if (this.createIssueForm.invalid) {
        this.createIssueForm.markAllAsTouched();
        return;
    }
    
    const projectId = this.createIssueForm.get('projectId')?.value;
    if (!projectId) {
      console.error('Project is not selected');
      return;
    }

    const formValue = this.createIssueForm.getRawValue();

    const newIssue: Partial<Issue> = {
      projectId: formValue.projectId,
      issueType: formValue.issueType,
      summary: formValue.summary,
      description: formValue.description,
      status: formValue.status,
      reporterId: formValue.reporterId, // Replace with actual user ID
    };

    this.issueService.createIssue(newIssue as Issue).subscribe({
        next: (createdIssue) => {
            console.log('Issue created successfully', createdIssue);
            if (!formValue.createAnother) {
                this.dialogRef.close(createdIssue);
            } else {
                this.createIssueForm.get('summary')?.reset();
                this.createIssueForm.get('description')?.reset();
            }
        },
        error: (err) => {
            console.error('Error creating issue:', err);
        }
    });
  }
}
