import { Component, OnInit, OnDestroy } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ActivatedRoute, Router, RouterModule } from "@angular/router";
import { TranslateModule, TranslateService } from "@ngx-translate/core";
import { MatTabsModule } from "@angular/material/tabs";
import { MatCardModule } from "@angular/material/card";
import { MatProgressBarModule } from "@angular/material/progress-bar";
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";
import { MatSnackBar, MatSnackBarModule } from "@angular/material/snack-bar";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";

import { UserAPIService } from "@/shared/services/user-api.service";
import { DynamicAvatarComponent } from "@/shared/components/dynamic-avatar/dynamic-avatar.component";
import { AvatarStage } from "@/models/profile/avatar-stage.enum";
import { IProfileStats } from "@/models/profile/IProfileStats";
import { ProfileOverviewTabComponent } from "../profile-overview-tab/profile-overview-tab.component";
import { ProfileJournalTabComponent } from "../profile-journal-tab/profile-journal-tab.component";
import { ProfileAchievementsTabComponent } from "../profile-achievements-tab/profile-achievements-tab.component";
import { ProfileSettingsTabComponent } from "../profile-settings-tab/profile-settings-tab.component";

const PROFILE_TAB_SLUGS = ["overview", "journal", "achievements", "settings"] as const;

@Component({
  selector: "nat-profile-page",
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    TranslateModule,
    MatTabsModule,
    MatCardModule,
    MatProgressBarModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    DynamicAvatarComponent,
    ProfileOverviewTabComponent,
    ProfileJournalTabComponent,
    ProfileAchievementsTabComponent,
    ProfileSettingsTabComponent,
  ],
  templateUrl: "./profile-page.component.html",
  styleUrls: ["./profile-page.component.css"],
})
export class ProfilePageComponent implements OnInit, OnDestroy {
  readonly user$ = this.userService.$user;
  profileStats: IProfileStats | null = null;
  statsLoading = false;
  selectedTabIndex = 0;
  private destroy$ = new Subject<void>();

  constructor(
    private userService: UserAPIService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar,
    private translate: TranslateService,
  ) {}

  ngOnInit(): void {
    this.userService.refreshCurrentUser().pipe(takeUntil(this.destroy$)).subscribe();
    this.loadProfileStats();
    this.userService
      .recordProfileVisit()
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.loadProfileStats();
      });
    this.route.queryParams.pipe(takeUntil(this.destroy$)).subscribe((params) => {
      const tab = params["tab"];
      const index = PROFILE_TAB_SLUGS.indexOf(tab);
      this.selectedTabIndex = index >= 0 ? index : 0;
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onTabChange(index: number): void {
    const slug = PROFILE_TAB_SLUGS[index];
    if (slug == null) return;
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { tab: slug },
      queryParamsHandling: "merge",
      replaceUrl: false,
    });
  }

  loadProfileStats(): void {
    this.statsLoading = true;
    this.userService
      .getProfileStats()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (stats) => {
          this.profileStats = stats;
          this.statsLoading = false;
        },
        error: () => {
          this.statsLoading = false;
          this.snackBar.open(this.translate.instant("profile.statsLoadError"), this.translate.instant("common.close"), {
            duration: 4000,
          });
        },
      });
  }

  getAvatarStage(): AvatarStage {
    if (!this.profileStats?.avatarStage) return AvatarStage.Seed;
    const s = this.profileStats.avatarStage;
    if (Object.values(AvatarStage).includes(s as AvatarStage)) return s as AvatarStage;
    return AvatarStage.Seed;
  }

  getLevelProgress(): number {
    if (!this.profileStats) return 0;
    const { xpCurrent, xpToNextLevel } = this.profileStats;
    const span = xpCurrent + xpToNextLevel;
    return span > 0 ? (xpCurrent / span) * 100 : 100;
  }
}
