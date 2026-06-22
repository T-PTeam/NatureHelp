import { Component, OnInit, OnDestroy } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { NavigationEnd, Router } from "@angular/router";
import { Observable, Subject } from "rxjs";
import { filter, map, startWith, takeUntil } from "rxjs/operators";
import moment from "moment";

import { AuthDialogService } from "@/shared/services/auth-dialog.service";
import { UserAPIService } from "@/shared/services/user-api.service";
import { EmailVerificationService } from "@/shared/services/email-verification.service";
import { MobileMapService } from "@/shared/services/mobile-map.service";

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
    public userService: UserAPIService,
    private emailVerificationService: EmailVerificationService,
    private notify: MatSnackBar,
    public mobileMapService: MobileMapService,
    private router: Router,
    private authDialogService: AuthDialogService,
  ) {}

  get isMobile(): boolean {
    return this.mobileMapService.isMobile();
  }

  isRouteActive(route: string): Observable<boolean> {
    return this.router.events.pipe(
      filter((event) => event instanceof NavigationEnd),
      map((event) => {
        const url = event.url;
        return url.includes(route);
      }),
      startWith(this.router.url.includes(route)),
    );
  }

  isDeficienciesRouteActive(): Observable<boolean> {
    return this.router.events.pipe(
      filter((event) => event instanceof NavigationEnd),
      map((event) => {
        const url = event.url;
        return url.includes("water") || url.includes("soil") || url === "/";
      }),
      startWith(this.router.url.includes("water") || this.router.url.includes("soil") || this.router.url === "/"),
    );
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
    this.authDialogService.openAuthDialog(isRegister);
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
