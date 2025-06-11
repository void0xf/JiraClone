import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectBoardNewItemTextAreaComponent } from './project-board-new-item-text-area.component';

describe('ProjectBoardNewItemTextAreaComponent', () => {
  let component: ProjectBoardNewItemTextAreaComponent;
  let fixture: ComponentFixture<ProjectBoardNewItemTextAreaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectBoardNewItemTextAreaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectBoardNewItemTextAreaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
