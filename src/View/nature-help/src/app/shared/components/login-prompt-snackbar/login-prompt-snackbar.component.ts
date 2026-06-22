import { Component, Inject } from "@angular/core";
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from "@angular/material/snack-bar";

import { AuthDialogService } from "@/shared/services/auth-dialog.service";

@Component({
  selector: "nat-login-prompt-snackbar",
  templateUrl: "./login-prompt-snackbar.component.html",
  styleUrls: ["./login-prompt-snackbar.component.css"],
  standalone: false,
})
export class LoginPromptSnackbarComponent {
  constructor(
    @Inject(MAT_SNACK_BAR_DATA) public data: { message: string },
    private snackBarRef: MatSnackBarRef<LoginPromptSnackbarComponent>,
    private authDialogService: AuthDialogService,
  ) {}

  onLogin(): void {
    this.snackBarRef.dismiss();
    this.authDialogService.openAuthDialog(false);
  }

  onClose(): void {
    this.snackBarRef.dismiss();
  }
}
