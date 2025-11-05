// src/app/core/guards/auth.guard.ts
import { inject } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { AuthGuardData, createAuthGuard } from "keycloak-angular";

const isAccessAllowed = async (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot,
  authData: AuthGuardData
): Promise<boolean | UrlTree> => {
  const router = inject(Router);
  const { authenticated } = authData;
  if (authenticated) {
    return true;
  }
  return router.parseUrl("/jira/software/sign-in");
};

export const authGuard: CanActivateFn = createAuthGuard(isAccessAllowed);