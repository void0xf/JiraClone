import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideChevronRight } from '@ng-icons/lucide';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  viewProviders: [],
  template: ` <router-outlet></router-outlet> `,
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'frontend';
}
