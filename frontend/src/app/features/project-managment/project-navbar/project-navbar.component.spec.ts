import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectNavbarComponent } from './project-navbar.component';

describe('ProjectNavbarComponent', () => {
  let component: ProjectNavbarComponent;
  let fixture: ComponentFixture<ProjectNavbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectNavbarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectNavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
