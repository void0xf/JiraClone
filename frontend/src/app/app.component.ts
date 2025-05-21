import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideChevronRight } from '@ng-icons/lucide';
import Keycloak from 'keycloak-js';
import { KeycloakStatusService } from './core/services/keycloak.service';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  viewProviders: [],
  template: ` <router-outlet></router-outlet> `,
  styleUrl: './app.component.scss',
})
export class AppComponent {
  private keycloakStatusService = inject(KeycloakStatusService);

  keycloakStatus = this.keycloakStatusService.keycloakType;
  authenticated = this.keycloakStatusService.authenticated;

  title = 'frontend';
}
