import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignUpEmailPopupComponent } from './sign-up-email-popup.component';

describe('SignUpEmailPopupComponent', () => {
  let component: SignUpEmailPopupComponent;
  let fixture: ComponentFixture<SignUpEmailPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SignUpEmailPopupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SignUpEmailPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
