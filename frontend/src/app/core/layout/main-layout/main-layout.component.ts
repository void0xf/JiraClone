import { Component } from '@angular/core';
import { SidebarComponent } from "../../../shared/componenets/sidebar/sidebar.component";
import { HeaderComponent } from "../../../shared/componenets/header/header.component";
import { RouterOutlet } from "@angular/router";

@Component({
  selector: 'app-main-layout',
  imports: [SidebarComponent, HeaderComponent, RouterOutlet],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.scss'
})
export class MainLayoutComponent {

}
