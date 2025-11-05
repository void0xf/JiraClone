import { ChangeDetectionStrategy, Component, effect, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import Keycloak from 'keycloak-js';
import { KEYCLOAK_EVENT_SIGNAL, KeycloakEventType } from 'keycloak-angular';
import { Router } from '@angular/router';

@Component({
  selector: 'sign-in',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template:'',
  styleUrl: './sign-in.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SignInComponent  {
  private readonly keyclaok = inject(Keycloak);
  private readonly keyclaokSignal = inject(KEYCLOAK_EVENT_SIGNAL)
  private readonly router = inject(Router);
  
  constructor() {
    effect(() => {
      const event = this.keyclaokSignal();
      if(event.type == KeycloakEventType.Ready){
        if(this.keyclaok.authenticated) {
          this.router.navigate(['jira/software/for-you'])
        }
        else {
          this.keyclaok.login();
        }
      }
    });
  }
}
