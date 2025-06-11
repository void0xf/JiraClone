import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectBoardNewItemButtonComponent } from './project-board-new-item-button.component';

describe('ProjectBoardNewItemButtonComponent', () => {
  let component: ProjectBoardNewItemButtonComponent;
  let fixture: ComponentFixture<ProjectBoardNewItemButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectBoardNewItemButtonComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectBoardNewItemButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
