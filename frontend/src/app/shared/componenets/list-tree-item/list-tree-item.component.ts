import {
  Component,
  Input,
  Output,
  EventEmitter,
  HostBinding,
  HostListener,
  ElementRef,
  OnInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import { lucideFileText, lucidePlus } from '@ng-icons/lucide'; // Base icons
import * as lucideIcons from '@ng-icons/lucide'; // Import all for dynamic icon name
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { HlmIconDirective } from '@spartan-ng/ui-icon-helm';
import { TreeSize } from '../list-tree/list-tree.component';

@Component({
  selector: 'app-list-tree-item',
  standalone: true,
  imports: [
    CommonModule,
    NgIconComponent,
    HlmButtonDirective,
    HlmIconDirective,
  ],
  providers: [
    provideIcons({
      ...lucideIcons, // Provide all lucide icons
    }),
  ],
  templateUrl: './list-tree-item.component.html',
})
export class ListTreeItemComponent implements OnInit {
  @Input({ required: true }) label: string = '';
  @Input() icon: keyof typeof lucideIcons = 'lucideFileText'; // Allow any lucide icon name
  @Input() isActive: boolean = false;
  @Input() id: string = '';
  @Input() size: TreeSize = 'md'; // Default to medium size

  @Output() itemClick = new EventEmitter<void>();
  // Optional actions for the item itself
  @Output() addAction = new EventEmitter<MouseEvent>();
  @Output() moreActions = new EventEmitter<MouseEvent>();

  // Unique ID for this item
  itemId: string = '';

  constructor(private el: ElementRef) {}

  ngOnInit(): void {
    // Generate a unique ID if not provided
    this.itemId =
      this.id || `item-${Math.random().toString(36).substring(2, 9)}`;
  }

  // Method to set the size from parent
  setSize(size: TreeSize): void {
    this.size = size;
  }

  // Get icon size based on component size
  getIconSize(): string {
    switch (this.size) {
      case 'sm':
        return '12px';
      case 'lg':
        return '20px';
      case 'md':
      default:
        return '16px';
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

  // Apply Tailwind classes via HostBinding for size and active state
  @HostBinding('class')
  get elementClasses(): string {
    // Base classes
    const baseClasses =
      'group relative flex w-full cursor-pointer items-center rounded-md hover:bg-accent';

    // Active state classes
    const activeClasses = this.isActive
      ? 'bg-accent text-accent-foreground' // Use spartan/shadcn accent for active
      : 'text-foreground'; // Default text color

    // Size-specific classes
    let sizeClasses = '';
    switch (this.size) {
      case 'sm':
        sizeClasses = 'px-1.5 py-0.5 text-xs';
        break;
      case 'lg':
        sizeClasses = 'px-3 py-2 text-base';
        break;
      case 'md':
      default:
        sizeClasses = 'px-2 py-1 text-sm';
        break;
    }

    return `${baseClasses} ${activeClasses} ${sizeClasses}`;
  }

  // ARIA attributes for accessibility
  @HostBinding('attr.role') role = 'treeitem';

  @HostBinding('attr.id')
  get hostId(): string {
    return this.itemId;
  }

  @HostBinding('attr.aria-selected')
  get ariaSelected(): boolean {
    return this.isActive;
  }

  @HostBinding('attr.tabindex')
  get tabIndex(): number {
    return 0;
  }

  @HostBinding('class.tree-item-size-sm')
  get isSizeSmall(): boolean {
    return this.size === 'sm';
  }

  @HostBinding('class.tree-item-size-md')
  get isSizeMedium(): boolean {
    return this.size === 'md';
  }

  @HostBinding('class.tree-item-size-lg')
  get isSizeLarge(): boolean {
    return this.size === 'lg';
  }

  // Emit click only if not clicking an action button within the item
  @HostListener('click', ['$event'])
  onClick(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    if (!target.closest('button')) {
      // Check if the click originated from or within a button
      this.itemClick.emit();
    }
  }

  // Add keyboard navigation
  @HostListener('keydown', ['$event'])
  onKeyDown(event: KeyboardEvent): void {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault();
      this.itemClick.emit();
    }
  }

  // --- Action Emitters ---
  emitAdd(event: MouseEvent): void {
    event.stopPropagation(); // Prevent itemClick
    this.addAction.emit(event);
  }

  emitMore(event: MouseEvent): void {
    event.stopPropagation(); // Prevent itemClick
    this.moreActions.emit(event);
  }
}
