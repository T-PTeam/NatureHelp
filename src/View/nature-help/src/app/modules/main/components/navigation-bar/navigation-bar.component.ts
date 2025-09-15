import { Component, OnInit, OnDestroy } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Observable, Subject } from "rxjs";
import { map, takeUntil } from "rxjs/operators";
import moment from "moment";

import { AuthDialogComponent } from "@/shared/components/dialogs/login-dialog/auth-dialog.component";
import { UserAPIService } from "@/shared/services/user-api.service";
import { EmailVerificationService } from "@/shared/services/email-verification.service";
import { MobileMapService } from "@/shared/services/mobile-map.service";
import { EAuthType } from "@/models/enums";

@Component({
  selector: "n-navigation-bar",
  templateUrl: "./navigation-bar.component.html",
  styleUrls: ["./navigation-bar.component.css"],
  standalone: false,
})
export class NavigationBarComponent implements OnInit, OnDestroy {
  public isOpenedMapOnMobile: boolean = false;
  private lastVerificationSent: number = 0;
  private destroy$ = new Subject<void>();
  public timeRemaining: number = 0;

  constructor(
    private dialog: MatDialog,
    public userService: UserAPIService,
    private emailVerificationService: EmailVerificationService,
    private notify: MatSnackBar,
    public mobileMapService: MobileMapService,
  ) {}

  get isMobile(): boolean {
    return this.mobileMapService.isMobile();
  }

  get isEmailVerificationDisabled$(): Observable<boolean> {
    return this.userService.$user.pipe(
      map((user) => {
        if (!user?.email) return true;
        if (user.isEmailConfirmed) return true;
        return this.timeRemaining > 0;
      }),
    );
  }

  get formattedTimeRemaining(): string {
    if (this.timeRemaining <= 0) return "";
    const duration = moment.duration(this.timeRemaining);
    const minutes = Math.floor(duration.asMinutes());
    const seconds = duration.seconds();
    return `${minutes}:${seconds.toString().padStart(2, "0")}`;
  }

  ngOnInit() {
    const savedTime = localStorage.getItem("lastVerificationSent");
    if (savedTime) {
      this.lastVerificationSent = parseInt(savedTime, 10);
    }

    this.startTimer();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private startTimer(): void {
    setInterval(() => {
      if (this.lastVerificationSent) {
        const elapsed = Date.now() - this.lastVerificationSent;
        const cooldownPeriod = 5 * 60 * 1000;
        this.timeRemaining = Math.max(0, cooldownPeriod - elapsed);
      } else {
        this.timeRemaining = 0;
      }
    }, 1000);
  }

  openAuthDialog(isRegister: boolean): void {
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
        this.userService
          .auth(isRegister ? EAuthType.Register : EAuthType.Login, result.email, result.password)
          .subscribe({
            next: (authResponse) => {
              if (authResponse.user && !authResponse.user.isEmailConfirmed) {
                this.emailVerificationService.sendVerificationEmail(authResponse.user.email).subscribe({
                  next: () => {
                    this.lastVerificationSent = Date.now();
                    localStorage.setItem("lastVerificationSent", this.lastVerificationSent.toString());
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

  sendEmailVerification(): void {
    this.userService.$user.pipe(takeUntil(this.destroy$)).subscribe((user) => {
      if (user?.email) {
        this.emailVerificationService.sendVerificationEmail(user.email).subscribe({
          next: () => {
            this.lastVerificationSent = Date.now();
            localStorage.setItem("lastVerificationSent", this.lastVerificationSent.toString());
            this.notify.open(`Verification email sent to ${user.email}`, "Close", { duration: 3000 });
          },
          error: (error) => {
            console.error("Error sending verification email:", error);
            this.notify.open("Failed to send verification email. Please try again.", "Close", { duration: 3000 });
          },
        });
      }
    });
  }
}
