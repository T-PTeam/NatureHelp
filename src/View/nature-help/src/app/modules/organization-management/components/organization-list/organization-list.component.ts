import { Component, OnInit, OnDestroy } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Router } from "@angular/router";
import { Subject, takeUntil } from "rxjs";

import { IOrganization } from "@/models/IOrganization";
import { OrganizationAPIService } from "../../services/organization-api.service";

@Component({
  selector: "nat-organization-list",
  templateUrl: "./organization-list.component.html",
  styleUrls: ["./organization-list.component.css"],
  standalone: false,
})
export class OrganizationListComponent implements OnInit, OnDestroy {
  organizations: IOrganization[] = [];
  isLoading = false;
  private destroy$ = new Subject<void>();

  constructor(
    private organizationAPIService: OrganizationAPIService,
    private notify: MatSnackBar,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.loadOrganizations();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadOrganizations(): void {
    this.isLoading = true;
    this.organizationAPIService.loadOrganizations(-1);

    this.organizationAPIService.organizations$.pipe(takeUntil(this.destroy$)).subscribe({
      next: (organizations) => {
        this.organizations = organizations;
        this.isLoading = false;
      },
      error: (error) => {
        console.error("Error loading organizations:", error);
        this.notify.open("Failed to load organizations", "Close", { duration: 3000 });
        this.isLoading = false;
      },
    });
  }

  onCreateOrganization(): void {
    this.router.navigate(["/organizations/add"]);
  }

  onViewOrganization(organization: IOrganization): void {
    this.router.navigate(["/organizations", organization.id]);
  }

  onEditOrganization(organization: IOrganization): void {
    this.router.navigate(["/organizations", organization.id]);
  }

  onDeleteOrganization(organization: IOrganization): void {
    if (confirm(`Are you sure you want to delete "${organization.title}"?`)) {
      this.organizationAPIService
        .deleteOrganization(organization.id)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.notify.open("Organization deleted successfully", "Close", { duration: 3000 });
            this.loadOrganizations();
          },
          error: (error) => {
            console.error("Error deleting organization:", error);
            this.notify.open("Failed to delete organization", "Close", { duration: 3000 });
          },
        });
    }
  }
}
