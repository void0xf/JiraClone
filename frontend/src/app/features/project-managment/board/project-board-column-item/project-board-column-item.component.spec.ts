import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectBoardColumnItemComponent } from './project-board-column-item.component';

describe('ProjectBoardColumnItemComponent', () => {
  let component: ProjectBoardColumnItemComponent;
  let fixture: ComponentFixture<ProjectBoardColumnItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectBoardColumnItemComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectBoardColumnItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
