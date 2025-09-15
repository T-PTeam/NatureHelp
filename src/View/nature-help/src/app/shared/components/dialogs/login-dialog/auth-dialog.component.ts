import { Component } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef, MatDialog } from "@angular/material/dialog";
import { SendResetLinkDialogComponent } from "./send-reset-link-dialog.component";

@Component({
  selector: "nat-auth-dialog",
  templateUrl: "./auth-dialog.component.html",
  styleUrls: ["./auth-dialog.component.css", "../../../styles/dialog-styles.css"],
  standalone: false,
})
export class AuthDialogComponent {
  formGroup: FormGroup;
  hidePassword = true;

  constructor(
    private dialogRef: MatDialogRef<AuthDialogComponent>,
    private fb: FormBuilder,
    private dialog: MatDialog,
  ) {
    this.formGroup = this.fb.group({
      email: ["", [Validators.required, Validators.email]],
      password: ["", [Validators.required, Validators.minLength(6)]],
    });
  }

  onSubmit(): void {
    if (this.formGroup.valid) {
      this.dialogRef.close(this.formGroup.value);
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  onForgotPassword(event: Event): void {
    event.preventDefault();

    const dialogRef = this.dialog.open(SendResetLinkDialogComponent, {
      width: "fit-content",
      height: "fit-content",
      maxHeight: "80vh",
      maxWidth: "80vw",
      minHeight: "300px",
      minWidth: "400px",
      disableClose: true,
    });

    dialogRef.afterClosed().subscribe();
  }
}
