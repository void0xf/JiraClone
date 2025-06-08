import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectBoardColumnComponent } from './project-board-column.component';

describe('ProjectBoardColumnComponent', () => {
  let component: ProjectBoardColumnComponent;
  let fixture: ComponentFixture<ProjectBoardColumnComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectBoardColumnComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectBoardColumnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
