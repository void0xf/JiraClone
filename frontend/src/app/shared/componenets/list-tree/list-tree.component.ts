import {
  Component,
  Input,
  Output,
  EventEmitter,
  HostBinding,
  signal,
  ContentChildren,
  QueryList,
  AfterContentInit,
  OnInit,
  ElementRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { provideIcons } from '@ng-icons/core';
import {
  lucideChevronDown,
  lucideChevronRight,
  lucidePlus,
  lucideEllipsis,
  lucideMoveHorizontal,
  lucideFolder,
  lucideRocket,
  lucideCloudLightning,
  lucideLayoutList,
} from '@ng-icons/lucide'; // Import base icons
import * as lucideIcons from '@ng-icons/lucide'; // Import all for dynamic icon name
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { HlmIconDirective } from '@spartan-ng/ui-icon-helm';
import { NgIcon } from '@ng-icons/core';
import { ListTreeItemComponent } from '../list-tree-item/list-tree-item.component';

// Define available size options
export type TreeSize = 'sm' | 'md' | 'lg';

@Component({
  selector: 'app-list-tree',
  standalone: true,
  imports: [CommonModule, HlmButtonDirective, HlmIconDirective, NgIcon],
  // Provide icons used directly in THIS template + a way to handle dynamic defaultIcon
  providers: [
    provideIcons({
      ...lucideIcons, // Provide all lucide icons
      lucideChevronDown,
      lucideChevronRight,
      lucidePlus,
      lucideEllipsis,
      lucideMoveHorizontal,
      lucideFolder,
      lucideRocket,
      lucideCloudLightning,
      lucideLayoutList,
    }),
  ],
  templateUrl: './list-tree.component.html',
  // No separate CSS file needed if using only Tailwind
})
export class ListTreeComponent implements OnInit, AfterContentInit {
  @Input({ required: true }) label: string = '';
  // Allow passing lucide icon names directly, e.g., 'lucideRocket'
  @Input() defaultIcon: keyof typeof lucideIcons = 'lucideFolder'; // Default icon name
  @Input() startExpanded: boolean = false;
  @Input() id: string = '';
  @Input() size: TreeSize = 'md'; // Default to medium size

  @Output() addAction = new EventEmitter<MouseEvent>();
  @Output() moreActions = new EventEmitter<MouseEvent>();

  @ContentChildren(ListTreeItemComponent, { descendants: true })
  treeItems!: QueryList<ListTreeItemComponent>;

  isExpanded = signal(this.startExpanded);
  isHovering = signal(false);

  // Unique ID for this tree to connect ARIA attributes
  treeId: string = '';

  constructor(private el: ElementRef) {
    this.isExpanded.set(this.startExpanded);
  }

  ngOnInit(): void {
    // Generate a unique ID if not provided
    this.treeId =
      this.id || `tree-${Math.random().toString(36).substring(2, 9)}`;
  }

  ngAfterContentInit(): void {
    // Logic for setting item.setSize has been removed
  }

  // Get CSS classes based on size
  getHeaderClasses(): string {
    const baseClasses =
      'group flex w-full cursor-pointer items-center rounded-md hover:bg-accent';

    // Size-specific classes
    let sizeClasses = '';
    switch (this.size) {
      case 'sm':
        sizeClasses = 'px-1.5 py-0.5 text-xs';
        break;
      case 'lg':
        sizeClasses = 'px-2 py-0.5 text-base';
        break;
      case 'md':
      default:
        sizeClasses = 'px-2 py-1 text-sm';
        break;
    }

    return `${baseClasses} ${sizeClasses}`;
  }

  // Get icon size based on component size
  getIconSize(): string {
    switch (this.size) {
      case 'sm':
        return '0.75rem';
      case 'lg':
        return '1.25rem';
      case 'md':
      default:
        return '1rem';
    }
  }

  // Get action button size classes
  getActionButtonClasses(): string {
    switch (this.size) {
      case 'sm':
        return 'h-5 w-5';
      case 'lg':
        return 'h-7 w-7';
      case 'md':
      default:
        return 'h-6 w-6';
    }
  }

  // Get content padding based on size
  getContentClasses(): string {
    const basePadding = 'pl-5 pt-1';

    switch (this.size) {
      case 'sm':
        return `${basePadding} text-xs`;
      case 'lg':
        return `${basePadding} text-base`;
      case 'md':
      default:
        return `${basePadding} text-sm`;
    }
  }

  toggleExpand(event: MouseEvent): void {
    // Prevent toggle if click is on an action button
    const target = event.target as HTMLElement;
    if (!target.closest('button')) {
      this.isExpanded.update((expanded) => !expanded);
    }
  }

  onKeyDown(event: KeyboardEvent): void {
    switch (event.key) {
      case 'Enter':
      case ' ': // Space key
        event.preventDefault();
        this.isExpanded.update((expanded) => !expanded);
        break;
      case 'ArrowRight':
        if (!this.isExpanded()) {
          event.preventDefault();
          this.isExpanded.set(true);
        }
        break;
      case 'ArrowLeft':
        if (this.isExpanded()) {
          event.preventDefault();
          this.isExpanded.set(false);
        }
        break;
    }
  }

  onHeaderEnter(): void {
    this.isHovering.set(true);
  }

  onHeaderLeave(): void {
    this.isHovering.set(false);
  }

  emitAdd(event: MouseEvent): void {
    event.stopPropagation();
    this.addAction.emit(event);
  }

  emitMore(event: MouseEvent): void {
    event.stopPropagation();
    this.moreActions.emit(event);
  }

  // HostBinding can still be useful for parent CSS selectors if needed
  @HostBinding('class.expanded') get expandedClass() {
    return this.isExpanded();
  }

  @HostBinding('class.collapsed') get collapsedClass() {
    return !this.isExpanded();
  }

  @HostBinding('attr.role') get role() {
    return 'tree';
  }

  @HostBinding('attr.aria-label') get ariaLabel() {
    return `${this.label} tree`;
  }

  @HostBinding('attr.id') get hostId() {
    return this.treeId;
  }

  @HostBinding('class') get sizeClass() {
    return `tree-size-${this.size}`;
  }
}
