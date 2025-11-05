import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sign-up-email-popup',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './sign-up-email-popup.component.html',
  styleUrl: './sign-up-email-popup.component.scss'
})
export class SignUpEmailPopupComponent {
  @Input() email: string = '';
}
