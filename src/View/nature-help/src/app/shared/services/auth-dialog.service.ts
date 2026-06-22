import { Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";

import { AuthDialogComponent } from "@/shared/components/dialogs/login-dialog/auth-dialog.component";
import { LoginPromptSnackbarComponent } from "@/shared/components/login-prompt-snackbar/login-prompt-snackbar.component";
import { EmailVerificationService } from "@/shared/services/email-verification.service";
import { PostAuthWelcomeDialogService } from "@/shared/services/post-auth-welcome-dialog.service";
import { UserAPIService } from "@/shared/services/user-api.service";
import { getErrorMessage } from "@/shared/utils/error.utils";
import { EAuthType } from "@/models/enums";

@Injectable({
  providedIn: "root",
})
export class AuthDialogService {
  isAuthenticating = false;

  constructor(
    private dialog: MatDialog,
    private userService: UserAPIService,
    private emailVerificationService: EmailVerificationService,
    private notify: MatSnackBar,
    private postAuthWelcomeDialog: PostAuthWelcomeDialogService,
  ) {}

  showLoginPrompt(message: string): void {
    this.notify.openFromComponent(LoginPromptSnackbarComponent, {
      data: { message },
      duration: 6000,
      horizontalPosition: "center",
      verticalPosition: "bottom",
    });
  }

  openAuthDialog(isRegister = false): void {
    const dialogRef = this.dialog.open(AuthDialogComponent, {
      width: "fit-content",
      height: "fit-content",
      maxHeight: "80vh",
      maxWidth: "80vw",
      minHeight: "300px",
      minWidth: "400px",
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.isAuthenticating = true;
        const refCode = isRegister ? new URLSearchParams(window.location.search).get("ref") : null;
        this.userService
          .auth(isRegister ? EAuthType.Register : EAuthType.Login, result.email, result.password, refCode)
          .subscribe({
            next: (authResponse) => {
              this.isAuthenticating = false;
              if (!authResponse?.user) {
                return;
              }
              this.postAuthWelcomeDialog.open(authResponse.user);
              if (!authResponse.user.isEmailConfirmed) {
                this.emailVerificationService.sendVerificationEmail(authResponse.user.email).subscribe({
                  next: () => {
                    const lastVerificationSent = Date.now();
                    localStorage.setItem("lastVerificationSent", lastVerificationSent.toString());
                    this.notify.open(`Verification email sent to ${authResponse.user.email}`, "Close", {
                      duration: 3000,
                    });
                  },
                  error: (error) => {
                    console.error("Error sending verification email:", error);
                  },
                });
              }
            },
            error: (err: unknown) => {
              this.isAuthenticating = false;
              const errorMessage = getErrorMessage(err);
              this.notify.open(errorMessage, "Close", { duration: 3000 });
            },
          });
      }
    });
  }
}
