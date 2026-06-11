import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router } from "@angular/router";
import { Observable, map, catchError, of } from "rxjs";
import { MatSnackBar } from "@angular/material/snack-bar";

import { LabsAPIService } from "../../modules/laboratories/services/labs-api.service";
import { canEditLaboratory } from "../helpers/laboratory-access.helper";

@Injectable({
  providedIn: "root",
})
export class LaboratoryOwnerGuard implements CanActivate {
  constructor(
    private labsAPIService: LabsAPIService,
    private router: Router,
    private notify: MatSnackBar,
  ) {}

  canActivate(route: ActivatedRouteSnapshot): Observable<boolean> | boolean {
    const labId = route.params["id"];

    if (!labId) {
      return true;
    }

    const currentUserId = sessionStorage.getItem("userId");
    if (!currentUserId) {
      this.showPermissionError("Please login to continue");
      this.router.navigate(["/unauthorized"]);
      return false;
    }

    if (sessionStorage.getItem("laboratoryId") === labId) {
      return true;
    }

    const role = sessionStorage.getItem("role")?.toLowerCase();
    if (role === "superadmin") {
      return true;
    }

    return this.labsAPIService.getLabById(labId).pipe(
      map((lab) => {
        if (canEditLaboratory(lab, currentUserId, role)) {
          return true;
        }

        this.showPermissionError("You can only edit laboratories that you created or belong to");
        this.router.navigate(["/labs"]);
        return false;
      }),
      catchError(() => {
        this.showPermissionError("Laboratory not found");
        this.router.navigate(["/labs"]);
        return of(false);
      }),
    );
  }

  private showPermissionError(message: string): void {
    this.notify.open(message, "Close", { duration: 3000 });
  }
}
