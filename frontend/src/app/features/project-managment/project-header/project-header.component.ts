import { Component, Input, input } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideEllipsis } from '@ng-icons/lucide';
import { HlmIconDirective } from '@spartan-ng/ui-icon-helm';

@Component({
  selector: 'app-project-header',
  standalone: true,
  imports: [HlmIconDirective, NgIcon],
  providers: [
    provideIcons({
      lucideEllipsis,
    }),
  ],
  templateUrl: './project-header.component.html',
  styleUrl: './project-header.component.scss',
})
export class ProjectHeaderComponent {
  @Input() projectName: string = 'Car Rental Web App';
}
