import { Component, computed, input } from '@angular/core';
import { BrnToggleDirective } from '@spartan-ng/brain/toggle';
import { cva, type VariantProps } from 'class-variance-authority';
import { hlm } from '@spartan-ng/brain/core';
import { ClassValue } from 'clsx';

export const sidebarToggleVariants = cva(
  'flex items-center w-full border-l-2 rounded-xs border-transparent text-left text-foreground hover:bg-background-hover hover:text-foreground-hover focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2',
  {
    variants: {
      variant: {
        default: `group-data-[state=on]:border-foreground-active group-data-[state=on]:bg-background-active group-data-[state=on]:text-foreground-active`,
      },
      size: {
        default: 'px-3 py-2 text-base',
        sm: 'px-3 py-2 text-sm',
        lg: 'px-5 py-4 text-lg',
      },
    },
    defaultVariants: {
      variant: 'default',
      size: 'default',
    },
  },
);

export type SidebarToggleVariants = VariantProps<typeof sidebarToggleVariants>;

@Component({
  selector: 'app-jira-sidebar-toggle',
  standalone: true,
  hostDirectives: [
    {
      directive: BrnToggleDirective,
      inputs: ['value', 'disabled'],
    },
  ],
  host: {
    class: 'block group',
  },
  template: `
    <button [class]="_computedClass()" type="button">
      <ng-content />
    </button>
  `,
})
export class JiraSidebarToggleComponent {
  public variant = input<SidebarToggleVariants['variant']>('default');
  public size = input<SidebarToggleVariants['size']>('default');
  public userClass = input<ClassValue>('', { alias: 'class' });

  protected _computedClass = computed(() =>
    hlm(sidebarToggleVariants({ variant: this.variant(), size: this.size() }), this.userClass()),
  );
}
