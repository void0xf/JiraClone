import { Component, inject, effect } from '@angular/core';
import { NgIf } from '@angular/common';
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
import { ProjectService } from '../../../core/services/project.service';
import { Observable } from 'rxjs';
import { Projects } from '../../../core/models/project.model';
import { ApiResponse } from '../../../core/models/api-response.model';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    NgIf,
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
export class SidebarComponent {
  SidebarItemType = SidebarItemType;
  activeItem: string | null = 'temple-run';
  projectsData: Projects[] | null = null;
  projectsIsLoading: boolean = true;
  projectService = inject(ProjectService);
  constructor(
    private sidebarService: SidebarService,
    private selectionService: SidebarSelectionService
  ) {
    this.selectionService.selectItem('1', SidebarItemType.PROJECT);
    effect(() => {
      this.projectService.getProjects().subscribe({
        next: (projects: Projects[]) => {
          this.projectsData = projects;
          this.projectsIsLoading = false;
        },
        error: (err) => {
          console.log(err, 'sidbear');
        },
      });
    });
  }

  get isOpen() {
    return this.sidebarService.isOpen;
  }

  selectMenuItem(id: string, type: SidebarItemType): void {
    this.selectionService.selectItem(id, type);
  }

  isMenuItemSelected(id: string): boolean {
    return this.selectionService.isItemSelected(id, SidebarItemType.MENU_ITEM);
  }

  handleItemClick(itemName: string): void {
    console.log('Item clicked:', itemName);
    this.activeItem = itemName;
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
