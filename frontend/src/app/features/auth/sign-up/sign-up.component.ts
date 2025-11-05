import { Component, effect, inject, signal } from '@angular/core';
import {
  KEYCLOAK_EVENT_SIGNAL,
  KeycloakEventType,
  ReadyArgs,
  typeEventArgs,
} from 'keycloak-angular';

import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import Keycloak from 'keycloak-js';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { SignUpEmailPopupComponent } from './components/sign-up-email-popup/sign-up-email-popup.component';
import { envirovment } from '../../../../../environments/environment';
import { Router } from '@angular/router';

type SocialProvider = {
  id: 'google'
  label: string;
  icon: string;
};

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, SignUpEmailPopupComponent],
  templateUrl: './sign-up.component.html'
})
export class SignUpComponent {
  private readonly keycloak = inject(Keycloak);
  private readonly keycloakSignal = inject(KEYCLOAK_EVENT_SIGNAL);
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  authenticated = signal(false);
  showEmailPopup = signal(false);
  submittedEmail = signal('');
  errorMessage = signal('');

  constructor() {
    effect(() => {
      const keycloakEvent = this.keycloakSignal();

      if (keycloakEvent.type === KeycloakEventType.Ready) {
        this.authenticated.set(typeEventArgs<ReadyArgs>(keycloakEvent.args));
        if(this.authenticated()) {
          this.router.navigate(['jira/software/for-you'])
        }
      }

      if (keycloakEvent.type === KeycloakEventType.AuthLogout) {
        this.authenticated.set(false);
      }
    });
  }

  public signUp() {
    this.keycloak.register();
  }



  private readonly fb = inject(FormBuilder);

  readonly currentYear = new Date().getFullYear();

  isSubmitting = false;
  busyProvider: SocialProvider['id'] | null = null;

  readonly signUpForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    updatesOptIn: [true]
  });

  readonly socialProviders: SocialProvider[] = [
    { id: 'google', label: 'Google', icon: '/assets/auth/google-icon.svg' },
  ];

  get emailControl() {
    return this.signUpForm.controls.email;
  }

  onSubmit(): void {
    if (this.signUpForm.invalid) {
      this.signUpForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage.set('');

    const email = this.emailControl.value;

    type SignUpSuccess = { keycloakRegistrationUrl: string };
    type ApiResponse<T> = {
      isSuccess: boolean;
      data?: T;
      error?: { message?: string; userMessage?: string };
    };

    this.http
      .post<ApiResponse<SignUpSuccess>>(
        `${envirovment.userApiBaseUrl}/api/v1/user/signup`,
        { email }
      )
      .subscribe({
        next: (response: ApiResponse<SignUpSuccess>) => {
          this.isSubmitting = false;

          if (response?.isSuccess && response.data?.keycloakRegistrationUrl) {
            window.location.href = response.data.keycloakRegistrationUrl;
            return;
          }

          const fallbackMessage = 'Unable to start registration. Please try again.';
          const apiMessage = response?.error?.userMessage || response?.error?.message;
          this.errorMessage.set(apiMessage || fallbackMessage);
        },
  error: (error: HttpErrorResponse) => {
          this.isSubmitting = false;
          const fallbackMessage = 'An error occurred. Please try again.';
          const apiMessage =
            error.error?.error?.userMessage ||
            error.error?.error?.message ||
            error.error?.detail;
          this.errorMessage.set(apiMessage || fallbackMessage);
        },
      });
  }

  handleSocialSignUp(provider: SocialProvider['id']): void {
    this.busyProvider = provider;
    // TODO: integrate with social auth flow.
    setTimeout(() => {
      this.busyProvider = null;
    }, 600);
  }
}
