import { Component } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";
import { EmailVerificationService } from "@/shared/services/email-verification.service";

@Component({
  selector: "nat-email-verification-dialog",
  templateUrl: "./email-verification-dialog.component.html",
  styleUrls: ["./email-verification-dialog.component.css"],
  standalone: false,
})
export class EmailVerificationDialogComponent {
  formGroup: FormGroup;
  isLoading = false;

  constructor(
    private dialogRef: MatDialogRef<EmailVerificationDialogComponent>,
    private fb: FormBuilder,
    private emailVerificationService: EmailVerificationService,
    private notify: MatSnackBar,
  ) {
    this.formGroup = this.fb.group({
      email: ["", [Validators.required, Validators.email]],
    });
  }

  onSubmit(): void {
    if (this.formGroup.valid) {
      this.isLoading = true;
      const email = this.formGroup.get("email")?.value;

      this.emailVerificationService.sendVerificationEmail(email).subscribe({
        next: (response) => {
          this.notify.open("Verification email sent successfully!", "Close", {
            duration: 3000,
          });
          this.dialogRef.close({ success: true, email });
        },
        error: (error) => {
          console.error("Error sending verification email:", error);
          this.notify.open("Failed to send verification email. Please try again.", "Close", { duration: 2000 });

          this.isLoading = false;
        },
      });
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
