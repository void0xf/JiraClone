import { Component,EventEmitter, ElementRef, Output, ViewChild } from '@angular/core';
import { FormsModule, NgModel } from '@angular/forms';
import { HlmInputDirective } from '@spartan-ng/ui-input-helm';

@Component({
  selector: 'app-project-board-new-item-text-area',
  imports: [HlmInputDirective, FormsModule],
  templateUrl: './project-board-new-item-text-area.component.html',
  styleUrl: './project-board-new-item-text-area.component.scss'
})
export class ProjectBoardNewItemTextAreaComponent {
  isHovering = false;
  issueName = ''
  @ViewChild('usernameInput') usernameInput!: ElementRef;
  @Output() focusLost = new EventEmitter<boolean>();
  ngAfterViewInit() {
    this.focusUsernameInput();
  }
  private focusUsernameInput() {
    if (this.usernameInput && this.usernameInput.nativeElement) {
      this.usernameInput.nativeElement.focus();
    } else {
    }
  }
  onEnterKeyPress() {
    if (this.issueName.trim() === '') {
    }

  }
   onBlur() {
    this.focusLost.emit(true);
   }

}
