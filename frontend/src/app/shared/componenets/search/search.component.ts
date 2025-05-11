import { Component, EventEmitter, Output, inject } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { HlmInputDirective } from '@spartan-ng/ui-input-helm';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { DestroyRef } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { lucideSearch, lucideCommand } from '@ng-icons/lucide';
import { provideIcons, NgIcon } from '@ng-icons/core';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [ReactiveFormsModule, HlmInputDirective, NgIcon],
  providers: [provideIcons({ lucideSearch, lucideCommand })],
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss'],
})
export class SearchComponent {
  private readonly destroyRef = inject(DestroyRef);

  public searchControl = new FormControl('');
  public isFocused = false;

  @Output() searchTerm = new EventEmitter<string>();

  constructor() {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        tap((value) => this.searchTerm.emit(value ?? '')),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }

  onSubmit(event: Event): void {
    event.preventDefault();
    this.searchTerm.emit(this.searchControl.value ?? '');
    console.log('Search submitted:', this.searchControl.value);
  }

  clearSearch(): void {
    this.searchControl.setValue('');
  }

  onFocus(): void {
    this.isFocused = true;
  }

  onBlur(): void {
    this.isFocused = false;
  }
}
