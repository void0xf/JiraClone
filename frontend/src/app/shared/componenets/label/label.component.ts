import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-label', // Keep selector simple
  standalone: true,
  imports: [CommonModule],
  template: `
    <div
      class="px-2 pb-1 pt-3 text-xs font-semibold uppercase text-muted-foreground"
    >
      {{ name }}
    </div>
  `,
  // Inline styles or Tailwind classes in template are sufficient
})
export class LabelComponent {
  @Input({ required: true }) name: string = '';
}
