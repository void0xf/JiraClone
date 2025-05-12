import { Component } from '@angular/core';
import { ProjectNavbarItemComponent } from '../project-navbar-item/project-navbar-item.component';
import * as lucideIcons from '@ng-icons/lucide';

type CateogryIconMap = {
  [key: string]: keyof typeof lucideIcons;
};

const CATEGORY_ICON_MAP: CateogryIconMap = {
  Summary: 'lucideGlobe',
  Timeline: 'lucideRows3',
  Backlog: 'lucideList',
  Board: 'lucideLayoutGrid',
  Calendar: 'lucideCalendar',
  List: 'lucideListChecks',
};

@Component({
  selector: 'app-project-navbar',
  imports: [ProjectNavbarItemComponent],
  templateUrl: './project-navbar.component.html',
  styleUrl: './project-navbar.component.scss',
})
export class ProjectNavbarComponent {
  categoryIconMap = CATEGORY_ICON_MAP;

  categoryEntries: [string, keyof typeof lucideIcons][];

  constructor() {
    this.categoryEntries = Object.entries(this.categoryIconMap);
  }

  trackCategoryEntry(
    index: number,
    entry: [string, keyof typeof lucideIcons]
  ): string {
    return entry[index];
  }
}
