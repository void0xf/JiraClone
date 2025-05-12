import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from '../../../shared/componenets/header/header.component';
import { SidebarComponent } from '../../../shared/componenets/sidebar/sidebar.component';
import { ProjectHeaderComponent } from '../../../features/project-managment/project-header/project-header.component';
import { ProjectNavbarComponent } from '../../../features/project-managment/project-navbar/project-navbar.component';

@Component({
  selector: 'app-project-layout',
  standalone: true,
  imports: [
    RouterOutlet,
    HeaderComponent,
    SidebarComponent,
    ProjectHeaderComponent,
    ProjectNavbarComponent,
  ],
  templateUrl: './project-layout.component.html',
  styleUrl: './project-layout.component.scss',
})
export class ProjectLayoutComponent {}
