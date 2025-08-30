import { Component, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";

import { AuthDialogComponent } from "@/shared/components/dialogs/login-dialog/auth-dialog.component";
import { EmailVerificationDialogComponent } from "@/shared/components/dialogs/email-verification-dialog/email-verification-dialog.component";
import { UserAPIService } from "@/shared/services/user-api.service";
import { MobileMapService } from "@/shared/services/mobile-map.service";
import { EAuthType } from "@/models/enums";

@Component({
  selector: "n-navigation-bar",
  templateUrl: "./navigation-bar.component.html",
  styleUrls: ["./navigation-bar.component.css"],
  standalone: false,
})
export class NavigationBarComponent implements OnInit {
  public isOpenedMapOnMobile: boolean = false;

  constructor(
    private dialog: MatDialog,
    public userService: UserAPIService,
    private notify: MatSnackBar,
    public mobileMapService: MobileMapService,
  ) {}

  get isMobile(): boolean {
    return this.mobileMapService.isMobile();
  }

  ngOnInit() {}

  openAuthDialog(isRegister: boolean): void {
    const dialogRef = this.dialog.open(AuthDialogComponent, {
      width: "60vw",
      maxWidth: "60vw",
      height: "300px",
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.userService
          .auth(isRegister ? EAuthType.Register : EAuthType.Login, result.email, result.password)
          .subscribe({
            error: (err) => {
              this.notify.open("Login failed", "Close", { duration: 2000 });
              return err;
            },
          });
      }
    });
  }

  logout() {
    this.userService.logout();
  }

  openEmailVerificationDialog(): void {
    const dialogRef = this.dialog.open(EmailVerificationDialogComponent, {
      width: "60vw",
      maxWidth: "60vw",
      height: "300px",
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result && result.success) {
        this.notify.open(`Verification email sent to ${result.email}`, "Close", { duration: 3000 });
      }
    });
  }
}
