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
        if (userRole) {
            if (userRole === "superadmin") return true;
            else if (includeRoles.includes(userRole) && !excludeRoles.includes(userRole)) return true;
            else {
                const message = "You do not have permission to access this page.";
                this.notify.open(message, "Close", { duration: 2000 });
                return false;
            }
        } else {
            const message = "You do not have permission to access this page.";
            this.notify.open(message, "Close", { duration: 2000 });
            return false;
        }
    }
}
