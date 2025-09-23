import { Component, OnInit, OnDestroy } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { MatSnackBar } from "@angular/material/snack-bar";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Subject, takeUntil } from "rxjs";

import { IOrganization } from "@/models/IOrganization";
import { IUser } from "@/models/IUser";
import { ERole } from "@/models/enums";
import { OrganizationAPIService } from "../../services/organization-api.service";
import { UserAPIService } from "@/shared/services/user-api.service";

@Component({
  selector: "nat-organization-detail",
  templateUrl: "./organization-detail.component.html",
  styleUrls: ["../../../../shared/styles/detail-page.component.css", "./organization-detail.component.css"],
  standalone: false,
})
export class OrganizationDetailComponent implements OnInit, OnDestroy {
  organization: IOrganization | null = null;
  isLoading = false;
  organizationForm: FormGroup;
  isSubmitting = false;
  private destroy$ = new Subject<void>();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private organizationAPIService: OrganizationAPIService,
    private userAPIService: UserAPIService,
    private notify: MatSnackBar,
    private fb: FormBuilder,
  ) {
    this.organizationForm = this.fb.group({
      title: ["", [Validators.required, Validators.minLength(2)]],
      allowedMembersCount: ["", [Validators.required, Validators.min(1)]],
      firstName: ["", [Validators.required, Validators.minLength(2)]],
      lastName: ["", [Validators.required, Validators.minLength(2)]],
      email: ["", [Validators.required, Validators.email]],
      password: ["", [Validators.required, Validators.minLength(6)]],
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      const id = params["id"];
      if (!id) {
        this.organization = null;
      } else {
        this.loadOrganization(id);
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadOrganization(id: string): void {
    this.isLoading = true;
    this.organizationAPIService
      .getOrganizationById(id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (organization) => {
          this.organization = organization;
          this.isLoading = false;
          this.populateForm(organization);
          this.clearOwnerValidators();
        },
        error: (error) => {
          console.error("Error loading organization:", error);
          this.notify.open("Failed to load organization details", "Close", { duration: 3000 });
          this.router.navigate(["/organizations"]);
        },
      });
  }

  private populateForm(organization: IOrganization): void {
    this.organizationForm.patchValue({
      title: organization.title,
      allowedMembersCount: organization.allowedMembersCount,
    });
  }

  private clearOwnerValidators(): void {
    this.organizationForm.get("firstName")?.clearValidators();
    this.organizationForm.get("lastName")?.clearValidators();
    this.organizationForm.get("email")?.clearValidators();
    this.organizationForm.get("password")?.clearValidators();

    this.organizationForm.get("firstName")?.updateValueAndValidity();
    this.organizationForm.get("lastName")?.updateValueAndValidity();
    this.organizationForm.get("email")?.updateValueAndValidity();
    this.organizationForm.get("password")?.updateValueAndValidity();
  }

  onEdit(): void {
    if (this.organization) {
      this.router.navigate(["/organizations", this.organization.id]);
    }
  }

  onDelete(): void {
    if (this.organization && confirm(`Are you sure you want to delete "${this.organization.title}"?`)) {
      this.organizationAPIService
        .deleteOrganization(this.organization.id)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.notify.open("Organization deleted successfully", "Close", { duration: 3000 });
            this.router.navigate(["/organizations"]);
          },
          error: (error) => {
            console.error("Error deleting organization:", error);
            this.notify.open("Failed to delete organization", "Close", { duration: 3000 });
          },
        });
    }
  }

  onBack(): void {
    this.router.navigate(["/organizations"]);
  }

  onSubmit(): void {
    if (this.organizationForm.valid && !this.isSubmitting) {
      this.isSubmitting = true;
      const formData = this.organizationForm.value;

      if (this.organization) {
        const organizationData: IOrganization = {
          ...this.organization,
          title: formData.title,
          allowedMembersCount: formData.allowedMembersCount,
        };

        this.organizationAPIService
          .updateOrganization(this.organization.id, organizationData)
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: () => {
              this.isSubmitting = false;
              this.notify.open("Organization updated successfully!", "Close", { duration: 3000 });
              this.router.navigate(["/organizations"]);
            },
            error: (error) => {
              console.error("Error updating organization:", error);
              this.isSubmitting = false;
              this.notify.open("Failed to update organization. Please try again.", "Close", { duration: 3000 });
            },
          });
      } else {
        const organizationData: IOrganization = {
          title: formData.title,
          allowedMembersCount: formData.allowedMembersCount,
        } as IOrganization;

        this.organizationAPIService
          .createOrganization(organizationData)
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: (createdOrganization) => {
              const userData: IUser = {
                firstName: formData.firstName,
                lastName: formData.lastName,
                email: formData.email,
                password: formData.password,
                organizationId: createdOrganization.id,
                role: ERole.Owner,
              } as IUser;

              this.userAPIService
                .addUserToOrganization(userData, true)
                .pipe(takeUntil(this.destroy$))
                .subscribe({
                  next: () => {
                    this.isSubmitting = false;
                    this.notify.open("Organization and owner created successfully!", "Close", { duration: 3000 });
                    this.router.navigate(["/organizations"]);
                  },
                  error: (userError) => {
                    console.error("Error creating user:", userError);
                    this.isSubmitting = false;
                    this.notify.open("Organization created but failed to create owner. Please try again.", "Close", {
                      duration: 3000,
                    });
                  },
                });
            },
            error: (orgError) => {
              console.error("Error creating organization:", orgError);
              this.isSubmitting = false;
              this.notify.open("Failed to create organization. Please try again.", "Close", { duration: 3000 });
            },
          });
      }
    } else {
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched(): void {
    Object.keys(this.organizationForm.controls).forEach((key) => {
      const control = this.organizationForm.get(key);
      control?.markAsTouched();
    });
  }

  getFieldError(fieldPath: string): string {
    const field = this.organizationForm.get(fieldPath);
    if (field?.errors && field.touched) {
      if (field.errors["required"]) return "This field is required";
      if (field.errors["minlength"]) return `Minimum length is ${field.errors["minlength"].requiredLength}`;
      if (field.errors["min"]) return `Minimum value is ${field.errors["min"].min}`;
      if (field.errors["email"]) return "Please enter a valid email";
    }
    return "";
  }
}
