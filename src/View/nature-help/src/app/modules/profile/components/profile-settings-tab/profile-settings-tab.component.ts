import { Component, OnDestroy, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { TranslateModule } from "@ngx-translate/core";
import { MatCardModule } from "@angular/material/card";
import { MatSlideToggleModule } from "@angular/material/slide-toggle";
import { MatDividerModule } from "@angular/material/divider";
import { MatButtonModule } from "@angular/material/button";
import { MatSnackBar } from "@angular/material/snack-bar";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { FormsModule } from "@angular/forms";
import { Subject } from "rxjs";
import { finalize, takeUntil } from "rxjs/operators";

import { UserAPIService } from "@/shared/services/user-api.service";
import { TranslateService } from "@ngx-translate/core";

@Component({
  selector: "nat-profile-settings-tab",
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatCardModule,
    MatSlideToggleModule,
    MatDividerModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    FormsModule,
  ],
  templateUrl: "./profile-settings-tab.component.html",
  styleUrls: ["./profile-settings-tab.component.css"],
})
export class ProfileSettingsTabComponent implements OnInit, OnDestroy {
  emailNotifications = false;
  achievementAlerts = false;
  newsletter = false;
  profilePublic = false;
  saving = false;
  private destroy$ = new Subject<void>();

  constructor(
    private userApi: UserAPIService,
    private snackBar: MatSnackBar,
    private translate: TranslateService,
  ) {}

  ngOnInit(): void {
    this.userApi.$user.pipe(takeUntil(this.destroy$)).subscribe((user) => {
      if (!user) return;
      this.profilePublic = user.profileIsPublic ?? false;
      this.emailNotifications = user.emailNotificationsEnabled ?? false;
      this.achievementAlerts = user.achievementAlertsEnabled ?? false;
      this.newsletter = user.newsletterEnabled ?? false;
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  saveSettings(): void {
    this.saving = true;
    this.userApi
      .updateProfileSettings({
        profileIsPublic: this.profilePublic,
        emailNotificationsEnabled: this.emailNotifications,
        achievementAlertsEnabled: this.achievementAlerts,
        newsletterEnabled: this.newsletter,
      })
      .pipe(finalize(() => (this.saving = false)))
      .subscribe((user) => {
        if (user) {
          this.snackBar.open(
            this.translate.instant("profile.settings.saveSuccess"),
            this.translate.instant("common.close"),
            { duration: 3000 },
          );
        }
      });
  }
}
