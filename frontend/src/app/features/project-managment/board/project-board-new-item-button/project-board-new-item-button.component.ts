import { Component, EventEmitter, Output } from '@angular/core';
import { HlmButtonDirective } from '../../../../../../libs/ui/ui-button-helm/src/lib/hlm-button.directive';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucidePlus } from '@ng-icons/lucide';

@Component({
  selector: 'app-project-board-new-item-button',
  imports: [HlmButtonDirective, NgIcon],
  providers: [provideIcons({lucidePlus})],
  templateUrl: './project-board-new-item-button.component.html',
  styleUrl: './project-board-new-item-button.component.scss'
})
export class ProjectBoardNewItemButtonComponent {
  @Output() buttonClicked = new EventEmitter<boolean>();
    addNewIssueItem() {
    this.buttonClicked.emit(true);
  }
}
