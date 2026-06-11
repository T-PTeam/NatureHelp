import { Component, OnDestroy, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { TranslateModule } from "@ngx-translate/core";
import { MatCardModule } from "@angular/material/card";
import { MatIconModule } from "@angular/material/icon";
import { MatProgressBarModule } from "@angular/material/progress-bar";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";

import { IAchievement } from "@/models/profile/IAchievement";
import { UserAPIService } from "@/shared/services/user-api.service";

@Component({
  selector: "nat-profile-achievements-tab",
  standalone: true,
  imports: [CommonModule, TranslateModule, MatCardModule, MatIconModule, MatProgressBarModule],
  templateUrl: "./profile-achievements-tab.component.html",
  styleUrls: ["./profile-achievements-tab.component.css"],
})
export class ProfileAchievementsTabComponent implements OnInit, OnDestroy {
  badges: IAchievement[] = [];
  challenges: IAchievement[] = [];
  private destroy$ = new Subject<void>();

  constructor(private userApi: UserAPIService) {}

  ngOnInit(): void {
    this.userApi
      .getProfileAchievements()
      .pipe(takeUntil(this.destroy$))
      .subscribe((list) => {
        this.badges = list.filter((a) => (a.kind ?? 0) === 0);
        this.challenges = list.filter((a) => (a.kind ?? 0) === 1);
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
