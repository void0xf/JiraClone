import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from '../../../shared/componenets/header/header.component';
import { SidebarComponent } from '../../../shared/componenets/sidebar/sidebar.component';
import { ProjectHeaderComponent } from '../../../shared/componenets/project-header/project-header.component';

@Component({
  selector: 'app-project-layout',
  standalone: true,
  imports: [
    RouterOutlet,
    HeaderComponent,
    SidebarComponent,
    ProjectHeaderComponent,
  ],
  templateUrl: './project-layout.component.html',
  styleUrl: './project-layout.component.scss',
})
export class ProjectLayoutComponent {}
