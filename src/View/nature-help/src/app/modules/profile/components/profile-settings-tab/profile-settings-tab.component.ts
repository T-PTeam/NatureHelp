import { Component, OnDestroy, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { TranslateModule, TranslateService } from "@ngx-translate/core";
import { MatCardModule } from "@angular/material/card";
import { MatSlideToggleModule } from "@angular/material/slide-toggle";
import { MatDividerModule } from "@angular/material/divider";
import { MatButtonModule } from "@angular/material/button";
import { MatSnackBar } from "@angular/material/snack-bar";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatDialogModule, MatDialog } from "@angular/material/dialog";
import { MatInputModule } from "@angular/material/input";
import { MatFormFieldModule } from "@angular/material/form-field";
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { FormsModule } from "@angular/forms";
import { Router } from "@angular/router";
import { Subject } from "rxjs";
import { finalize, takeUntil } from "rxjs/operators";

import { UserAPIService } from "@/shared/services/user-api.service";

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
    MatDialogModule,
    MatInputModule,
    MatFormFieldModule,
    ReactiveFormsModule,
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
  deletingAccount = false;
  changingPassword = false;
  showPasswordForm = false;

  passwordForm: FormGroup;

  private destroy$ = new Subject<void>();

  constructor(
    private userApi: UserAPIService,
    private snackBar: MatSnackBar,
    private translate: TranslateService,
    private dialog: MatDialog,
    private router: Router,
    private fb: FormBuilder,
  ) {
    this.passwordForm = this.fb.group(
      {
        currentPassword: ["", [Validators.required]],
        newPassword: ["", [Validators.required, Validators.minLength(8)]],
        confirmPassword: ["", [Validators.required]],
      },
      { validators: this.passwordsMatchValidator },
    );
  }

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

  submitChangePassword(): void {
    if (this.passwordForm.invalid) return;
    this.changingPassword = true;
    const { currentPassword, newPassword } = this.passwordForm.value;
    this.userApi
      .changePassword(currentPassword, newPassword)
      .pipe(
        finalize(() => (this.changingPassword = false)),
        takeUntil(this.destroy$),
      )
      .subscribe((success) => {
        if (success) {
          this.snackBar.open(this.translate.instant("profile.settings.passwordChangeSuccess"), this.translate.instant("common.close"), { duration: 3000 });
          this.showPasswordForm = false;
          this.passwordForm.reset();
        }
      });
  }

  confirmDeleteAccount(): void {
    const confirmed = window.confirm(this.translate.instant("profile.settings.deleteConfirm"));
    if (!confirmed) return;
    this.deletingAccount = true;
    this.userApi
      .deleteCurrentUser()
      .pipe(
        finalize(() => (this.deletingAccount = false)),
        takeUntil(this.destroy$),
      )
      .subscribe((success) => {
        if (success) {
          this.userApi.logout();
          this.router.navigate(["/"]);
        }
      });
  }

  private passwordsMatchValidator(group: FormGroup): { passwordsMismatch: true } | null {
    const newPw = group.get("newPassword")?.value;
    const confirm = group.get("confirmPassword")?.value;
    return newPw && confirm && newPw !== confirm ? { passwordsMismatch: true } : null;
  }
}
