import { Component, Input, forwardRef, HostListener, ElementRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { CommonModule } from '@angular/common';

export interface DropdownOption {
  value: any;
  label: string;
  icon?: string;
}

export interface DropdownGroup {
  label: string;
  options: DropdownOption[];
}

@Component({
  selector: 'app-dropdown',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dropdown.component.html',
  styleUrls: ['./dropdown.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DropdownComponent),
      multi: true
    }
  ]
})
export class DropdownComponent implements ControlValueAccessor {
  @Input() groups: DropdownGroup[] = [];
  @Input() options: DropdownOption[] = [];
  @Input() placeholder: string = 'Select an option';

  selectedOption: DropdownOption | undefined;
  isOpen = false;
  
  private onChange = (value: any) => {};
  private onTouched = () => {};

  constructor(private _elementRef: ElementRef) {}

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (!this._elementRef.nativeElement.contains(event.target)) {
      this.isOpen = false;
      this.onTouched();
    }
  }

  get allOptions(): DropdownOption[] {
    if (this.options.length) {
      return this.options;
    }
    return this.groups.flatMap(g => g.options);
  }

  writeValue(value: any): void {
    this.selectedOption = this.allOptions.find(opt => opt.value === value);
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  selectOption(option: DropdownOption): void {
    this.selectedOption = option;
    this.onChange(option.value);
    this.onTouched();
    this.isOpen = false;
  }

  toggleDropdown(event: MouseEvent): void {
    event.stopPropagation();
    this.isOpen = !this.isOpen;
    if (!this.isOpen) {
      this.onTouched();
    }
  }
} 