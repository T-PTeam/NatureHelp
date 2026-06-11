import { Component, Input, OnInit, OnDestroy, ElementRef, ViewChild } from "@angular/core";
import { CommonModule } from "@angular/common";
import { TranslateModule, TranslateService } from "@ngx-translate/core";
import { MatCardModule } from "@angular/material/card";
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from "@angular/material/button";
import { MatSnackBar, MatSnackBarModule } from "@angular/material/snack-bar";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";
import { toPng } from "html-to-image";

import { DynamicAvatarComponent } from "@/shared/components/dynamic-avatar/dynamic-avatar.component";
import { IProfileStats } from "@/models/profile/IProfileStats";
import { IReferral } from "@/models/profile/IReferral";
import { UserAPIService } from "@/shared/services/user-api.service";

@Component({
  selector: "nat-profile-overview-tab",
  standalone: true,
  imports: [CommonModule, TranslateModule, MatCardModule, MatIconModule, MatButtonModule, MatSnackBarModule, DynamicAvatarComponent],
  templateUrl: "./profile-overview-tab.component.html",
  styleUrls: ["./profile-overview-tab.component.css"],
})
export class ProfileOverviewTabComponent implements OnInit, OnDestroy {
  @Input() stats: IProfileStats | null = null;
  @ViewChild("impactCard", { static: false }) impactCardRef!: ElementRef<HTMLElement>;

  referrals: IReferral[] = [];
  inviteLink: string | null = null;
  linkCopied = false;
  sharingImage = false;

  private destroy$ = new Subject<void>();

  constructor(
    private userApi: UserAPIService,
    private snackBar: MatSnackBar,
    private translate: TranslateService,
  ) {}

  ngOnInit(): void {
    this.userApi
      .getProfileReferrals()
      .pipe(takeUntil(this.destroy$))
      .subscribe((refs) => (this.referrals = refs));
    this.userApi
      .getReferralInviteLink()
      .pipe(takeUntil(this.destroy$))
      .subscribe((link) => (this.inviteLink = link));
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  copyInviteLink(): void {
    if (!this.inviteLink) return;
    navigator.clipboard.writeText(this.inviteLink).then(() => {
      this.linkCopied = true;
      setTimeout(() => (this.linkCopied = false), 2000);
    });
  }

  async shareProfileImage(): Promise<void> {
    if (!this.impactCardRef?.nativeElement) return;
    this.sharingImage = true;
    try {
      const dataUrl = await toPng(this.impactCardRef.nativeElement, { cacheBust: true });
      const link = document.createElement("a");
      link.download = "my-impact.png";
      link.href = dataUrl;
      link.click();
    } catch {
      this.snackBar.open(this.translate.instant("profile.overview.shareError"), this.translate.instant("common.close"), { duration: 3000 });
    } finally {
      this.sharingImage = false;
    }
  }
}
