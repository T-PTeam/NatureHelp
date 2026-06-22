import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { ActivatedRouteSnapshot, CanActivate } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { Observable } from "rxjs";

import { AuthDialogService } from "@/shared/services/auth-dialog.service";

@Injectable({
  providedIn: "root",
})
export class RoleGuard implements CanActivate {
  constructor(
    private notify: MatSnackBar,
    private authDialogService: AuthDialogService,
    private translate: TranslateService,
  ) {}

  canActivate(route: ActivatedRouteSnapshot): Observable<boolean> | boolean {
    const includeRoles = route.data["includeRoles"] || [];
    const excludeRoles = route.data["excludeRoles"] || [];

    const userRole = sessionStorage.getItem("role")?.toLowerCase();
    if (!userRole) {
      return this.showPermissionAccessError();
    }

    if (userRole === "superadmin") {
      return true;
    }

    if (includeRoles.length > 0) {
      if (includeRoles.includes(userRole)) {
        return true;
      } else {
        return this.showPermissionAccessError(true);
      }
    }

    if (excludeRoles.length > 0) {
      if (!excludeRoles.includes(userRole)) {
        return true;
      } else {
        return this.showPermissionAccessError(true);
      }
    }

    return true;
  }

  private showPermissionAccessError(isAuthorized: boolean = false): boolean {
    if (isAuthorized) {
      const message = this.translate.instant("auth.noPermission");
      this.notify.open(message, this.translate.instant("common.close"), { duration: 2000 });
      return false;
    }

    const message = this.translate.instant("auth.pleaseLoginToAccount");
    this.authDialogService.showLoginPrompt(message);
    return false;
  }
}
