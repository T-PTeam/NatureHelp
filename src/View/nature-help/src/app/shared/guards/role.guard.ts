import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { ActivatedRouteSnapshot, CanActivate } from "@angular/router";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class RoleGuard implements CanActivate {
  constructor (private notify: MatSnackBar) { }

  canActivate (route: ActivatedRouteSnapshot): Observable<boolean> | boolean {
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

  private showPermissionAccessError (isAuthorized: boolean = false): boolean {
    const message = isAuthorized ? "You do not have permission to access this page." : "Please, login to account";
    this.notify.open(message, "Close", { duration: 2000 });
    return false;
  }
}
