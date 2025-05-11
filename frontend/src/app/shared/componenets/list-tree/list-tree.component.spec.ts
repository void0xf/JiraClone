import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListTreeComponent } from './list-tree.component';

describe('ListTreeComponent', () => {
  let component: ListTreeComponent;
  let fixture: ComponentFixture<ListTreeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListTreeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListTreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
