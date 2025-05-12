import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectNavbarItemComponent } from './project-navbar-item.component';

describe('ProjectNavbarItemComponent', () => {
  let component: ProjectNavbarItemComponent;
  let fixture: ComponentFixture<ProjectNavbarItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectNavbarItemComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectNavbarItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
