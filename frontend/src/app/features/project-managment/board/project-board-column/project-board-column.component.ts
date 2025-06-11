import { Component, Input, ViewChild, OnInit, Output, EventEmitter, OnChanges, SimpleChanges, DoCheck } from '@angular/core';
import { CdkDragDrop, DragDropModule, moveItemInArray, transferArrayItem, CdkDropList, CdkDrag } from '@angular/cdk/drag-drop';
import { ProjectBoardColumnItemComponent } from "../project-board-column-item/project-board-column-item.component";
import { CommonModule, NgIf } from '@angular/common';
import { Issue, IssueType, IssueStatus, IssuePriority } from '../../../../models/issue.model';
import { ProjectBoardNewItemButtonComponent } from '../project-board-new-item-button/project-board-new-item-button.component';
import { ProjectBoardNewItemTextAreaComponent } from '../project-board-new-item-text-area/project-board-new-item-text-area.component';

// Define an interface for the item data - REMOVED BoardItem

@Component({
  selector: 'app-project-board-column',
  imports: [CommonModule, CdkDropList, CdkDrag, ProjectBoardColumnItemComponent, NgIf, DragDropModule, ProjectBoardNewItemButtonComponent, ProjectBoardNewItemTextAreaComponent],
  templateUrl: './project-board-column.component.html',
  styleUrl: './project-board-column.component.scss',
})
export class ProjectBoardColumnComponent implements OnInit, DoCheck {
  @Input() columnTitle: string = 'ToDo';
  @Input() items: Issue[] = [];
  @Input() isDoneColumn: boolean = false;
  @Output() itemDropped = new EventEmitter<CdkDragDrop<Issue[]>>();
  isHovering = false;
  newItemButtonClicked = false;

  itemCount: number = 0;

  constructor() {
  }
  newItemButtonClick() {
    this.newItemButtonClicked = true;
  }
  focusLost() {
    this.newItemButtonClicked = false;
  }

  ngOnInit(): void {
    this.updateItemCount(); 
  }
  ngDoCheck(): void {
   this.updateItemCount();
  }

  onMouseEnter() {
    console.log('hovering')
    this.isHovering = true;
  }
  onMouseLeave() {
    this.isHovering = false;
  }

  updateItemCount(): void {
    this.itemCount = this.items.length;
  }

  drop(event: CdkDragDrop<Issue[]>) {
    this.itemDropped.emit(event);
    this.updateItemCount();
  }

  trackByIssueId(index: number, item: Issue): string {
    return item.id;
  }
}
