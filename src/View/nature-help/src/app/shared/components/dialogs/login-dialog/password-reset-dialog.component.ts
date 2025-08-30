import { Component } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { UserAPIService } from "../../../services/user-api.service";

@Component({
  selector: "nat-password-reset",
  templateUrl: "./password-reset-dialog.component.html",
  styleUrls: ["./password-reset-dialog.component.css"],
  standalone: false,
})
export class PasswordResetDialogComponent {
  formGroup: FormGroup;
  hidePassword = true;
  hideConfirmPassword = true;
  token: string | null = null;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private userApiService: UserAPIService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.formGroup = this.fb.group(
      {
        newPassword: ["", [Validators.required, Validators.minLength(6)]],
        confirmPassword: ["", [Validators.required]],
      },
      { validators: this.passwordMatchValidator },
    );

    this.route.queryParams.subscribe((params) => {
      this.token = params["token"];
      if (!this.token) {
        console.error("No reset token provided");
        this.router.navigate(["/"]);
      }
    });
  }

  passwordMatchValidator(formGroup: FormGroup) {
    const newPassword = formGroup.get("newPassword")?.value;
    const confirmPassword = formGroup.get("confirmPassword")?.value;

    if (newPassword !== confirmPassword) {
      formGroup.get("confirmPassword")?.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    } else {
      formGroup.get("confirmPassword")?.setErrors(null);
      return null;
    }
  }

  onSubmit(): void {
    if (this.formGroup.valid && this.token) {
      this.isLoading = true;
      const newPassword = this.formGroup.get("newPassword")?.value;

      this.userApiService.resetPassword(newPassword, this.token!).subscribe(
        (success) => {
          this.isLoading = false;
          if (success) {
            this.router.navigate(["/"]);
          }
        },
        (error) => {
          this.isLoading = false;
        },
      );
    }
  }

  goBack(): void {
    this.router.navigate(["/"]);
  }
}
