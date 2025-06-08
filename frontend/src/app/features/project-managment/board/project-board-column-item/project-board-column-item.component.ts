import { Component, Input, OnInit } from '@angular/core';
import { Issue, IssuePriority, IssueType } from '../../../../models/issue.model';
import { CommonModule, NgClass, NgIf } from '@angular/common';
import { CdkDrag } from '@angular/cdk/drag-drop';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideSquarePen } from '@ng-icons/lucide';

@Component({
  selector: 'app-project-board-column-item',
  templateUrl: './project-board-column-item.component.html',
  styleUrls: ['./project-board-column-item.component.scss'],
  standalone: true,
  imports: [CdkDrag, NgIf, NgClass, CommonModule, NgIcon],
  providers: [
    provideIcons({
      lucideSquarePen,
    }),
  ],
})
export class ProjectBoardColumnItemComponent implements OnInit {
  @Input() issue: Issue | null = null;

  name: string = '';
  taskId: string = '';
  assigneeAvatarUrl: string | undefined;
  showMoreOptions: boolean = true;
  isHovering: boolean = false;
  taskTypeIconUrl?: string;
  isTaskTypeCheckbox: boolean = true;
  showEditIcon: boolean = false;

  issueType = IssueType;
  issuePriority = IssuePriority;

  ngOnInit(): void {
    if (this.issue) {
      this.name = this.issue.summary;
      this.taskId = this.issue.key;
    }
  }
}
