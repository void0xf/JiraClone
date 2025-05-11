import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListTreeItemComponent } from './list-tree-item.component';

describe('ListTreeItemComponent', () => {
  let component: ListTreeItemComponent;
  let fixture: ComponentFixture<ListTreeItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListTreeItemComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListTreeItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
