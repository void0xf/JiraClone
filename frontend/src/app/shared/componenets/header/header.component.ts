import { Component, inject } from '@angular/core';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
// import { HlmSpinnerComponent } from-ng/ui-spinner-helm';
import { HlmIconDirective } from '@spartan-ng/ui-icon-helm';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {
  lucidePanelLeftClose,
  lucideLayoutGrid,
  lucidePlus,
  lucideCreditCard,
  lucideBell,
  lucideCircleHelp,
  lucideSettings,
  lucideSearch,
  lucideEllipsis,
  lucidePanelLeftOpen,
  lucidePanelRightOpen,
} from '@ng-icons/lucide';
import { SearchComponent } from '../search/search.component';
import { SidebarService } from '../../../core/services/sidebar.service';
import {
  HlmTooltipComponent,
  HlmTooltipTriggerDirective,
} from '@spartan-ng/ui-tooltip-helm';
import {Dialog, DialogRef, DIALOG_DATA, DialogModule} from '@angular/cdk/dialog';
import {CreateIssueDialogComponent} from '../../dialogs/create-issue-dialog/create-issue-dialog.component'
@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    HlmButtonDirective,
    HlmIconDirective,
    NgIcon,
    SearchComponent,
    HlmTooltipComponent,
    HlmTooltipTriggerDirective,
    HlmButtonDirective,
    DialogModule
  ],
  providers: [
    provideIcons({
      lucidePanelLeftClose,
      lucidePanelLeftOpen,
      lucidePanelRightOpen,
      lucideLayoutGrid,
      lucidePlus,
      lucideCreditCard,
      lucideBell,
      lucideCircleHelp,
      lucideSettings,
      lucideSearch,
      lucideEllipsis,
    }),
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent {
  constructor(private sidebarService: SidebarService) {}
  dialog = inject(Dialog);
  get isSidebarOpen() {
    return this.sidebarService.isOpen;
  }

  toggleSidebar(): void {
    this.sidebarService.toggle();
  }
  openCreateIssueDialog(): void{
    this.dialog.open(CreateIssueDialogComponent)

  }
}
