import { Injectable, signal } from '@angular/core';

export enum SidebarItemType {
  MENU_ITEM,
  PROJECT,
}

export interface SidebarSelection {
  id: string;
  type: SidebarItemType;
}

@Injectable({
  providedIn: 'root',
})
export class SidebarSelectionService {
  private selectedItem = signal<SidebarSelection | null>(null);

  selectItem(id: string, type: SidebarItemType): void {
    this.selectedItem.set({ id, type });
  }

  clearSelection(): void {
    this.selectedItem.set(null);
  }

  getSelectedItem() {
    return this.selectedItem;
  }

  isItemSelected(id: string, type: SidebarItemType): boolean {
    const current = this.selectedItem();
    return current?.id === id && current?.type === type;
  }
}
