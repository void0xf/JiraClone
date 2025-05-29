import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {
  provideClientHydration,
  withEventReplay,
} from '@angular/platform-browser';
import { provideKeycloakSSR } from './provide-keycloak-ssr';
import {
  AutoRefreshTokenService,
  CUSTOM_BEARER_TOKEN_INTERCEPTOR_CONFIG,
  customBearerTokenInterceptor,
  provideKeycloak,
  UserActivityService,
  withAutoRefreshToken,
} from 'keycloak-angular';
import { envirovment } from '../../environments/environment';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';

export const appConfig: ApplicationConfig = {
  providers: [
    // ❌ throws "window is not defined" on server
    // provideKeycloak({
    //   config: {
    //     url: 'http://localhost:8080',
    //     realm: 'realm-id',
    //     clientId: 'client-id',
    //   },
    //   initOptions: {
    //     onLoad: 'check-sso',
    //     silentCheckSsoRedirectUri:
    //       'http://localhost:4200/silent-check-sso.html',
    //   },
    //   features: [
    //     withAutoRefreshToken({
    //       onInactivityTimeout: 'logout',
    //       sessionTimeout: 60000,
    //     }),
    //   ],
    //   providers: [AutoRefreshTokenService, UserActivityService],
    // }),
    provideKeycloakSSR({
      config: {
        url: envirovment.keycloakSettings.url,
        realm: envirovment.keycloakSettings.realm,
        clientId: envirovment.keycloakSettings.clientId,
      },
      initOptions: {
        onLoad: 'check-sso',
        silentCheckSsoRedirectUri:
          'http://localhost:4200/silent-check-sso.html',
      },
      features: [
        withAutoRefreshToken({
          onInactivityTimeout: 'logout',
          sessionTimeout: 60000,
        }),
      ],
      providers: [AutoRefreshTokenService, UserActivityService],
    }),
    provideHttpClient(withInterceptors([customBearerTokenInterceptor])),
    {
      provide: CUSTOM_BEARER_TOKEN_INTERCEPTOR_CONFIG,
      useValue: [
        {
          shouldAddToken: async (req: any, _: any, keycloak: any) => {
            return req.url.startsWith('/') && keycloak.authenticated;
          },
        },
      ],
    },

    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideStore({}), // Provide root reducers here if any, otherwise empty object

    // NgRx Global Effects Setup
    provideEffects([]), // Provide root effects here if any, otherwise empty array

    // Optional: NgRx Store Devtools
    // Install with: npm install @ngrx/store-devtools --save

    provideClientHydration(withEventReplay()),
  ],
};
