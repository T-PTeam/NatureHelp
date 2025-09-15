import { Component } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { UserAPIService } from "../../../services/user-api.service";

@Component({
  selector: "nat-send-reset-link-dialog",
  templateUrl: "./send-reset-link-dialog.component.html",
  styleUrls: ["./send-reset-link-dialog.component.css", "../../../styles/dialog-styles.css"],
  standalone: false,
})
export class SendResetLinkDialogComponent {
  formGroup: FormGroup;

  constructor(
    private dialogRef: MatDialogRef<SendResetLinkDialogComponent>,
    private fb: FormBuilder,
    private userApiService: UserAPIService,
  ) {
    this.formGroup = this.fb.group({
      email: ["", [Validators.required, Validators.email]],
    });
  }

  onSubmit(): void {
    if (this.formGroup.valid) {
      const email = this.formGroup.get("email")?.value;
      this.userApiService.sendPasswordResetLink(email).subscribe((success) => {
        if (success) {
          this.dialogRef.close();
        }
      });
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
