import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { ActivatedRouteSnapshot, CanActivate } from "@angular/router";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class RoleGuard implements CanActivate {
  constructor(private notify: MatSnackBar) {}

  canActivate(route: ActivatedRouteSnapshot): Observable<boolean> | boolean {
    const includeRoles = route.data["includeRoles"] || [];
    const excludeRoles = route.data["excludeRoles"] || [];

    const userRole = localStorage.getItem("role")?.toLowerCase();
    console.log("ROLE: ", userRole);
    if (userRole) {
      if (userRole === "superadmin") return true;
      else if (includeRoles.includes(userRole) || !excludeRoles.includes(userRole)) return true;
      else {
        return this.showPermissionAccessError(true);
      }
    } else {
      return this.showPermissionAccessError();
    }
  }

  private showPermissionAccessError(isAuthorized: boolean = false): boolean {
    const message = isAuthorized ? "You do not have permission to access this page." : "Please, login to account";
    this.notify.open(message, "Close", { duration: 2000 });
    return false;
  }
}
