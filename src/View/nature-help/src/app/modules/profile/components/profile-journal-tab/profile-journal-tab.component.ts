import { Component, OnDestroy, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { TranslateModule } from "@ngx-translate/core";
import { MatCardModule } from "@angular/material/card";
import { MatIconModule } from "@angular/material/icon";
import { MatTableModule } from "@angular/material/table";
import { MatButtonModule } from "@angular/material/button";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";

import { EDeficiencyType } from "@/models/enums";
import { IProfileJournalEntry } from "@/models/profile/IProfileJournalEntry";
import { IProfilePhotoHistory } from "@/models/profile/IProfilePhotoHistory";
import { UserAPIService } from "@/shared/services/user-api.service";
import { LanguageService } from "@/shared/services/language.service";

@Component({
  selector: "nat-profile-journal-tab",
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule, MatCardModule, MatIconModule, MatTableModule, MatButtonModule],
  templateUrl: "./profile-journal-tab.component.html",
  styleUrls: ["./profile-journal-tab.component.css"],
})
export class ProfileJournalTabComponent implements OnInit, OnDestroy {
  readonly displayedColumns = ["type", "title", "createdOn", "address", "link"];
  journal: IProfileJournalEntry[] = [];
  photos: IProfilePhotoHistory[] = [];
  readonly deficiencyTypeEnum = EDeficiencyType;
  private destroy$ = new Subject<void>();

  constructor(
    private userApi: UserAPIService,
    private languageService: LanguageService,
  ) {}

  ngOnInit(): void {
    this.userApi
      .getProfileJournal()
      .pipe(takeUntil(this.destroy$))
      .subscribe((j) => {
        this.journal = j;
      });
    this.userApi
      .getProfilePhotoHistory()
      .pipe(takeUntil(this.destroy$))
      .subscribe((p) => {
        this.photos = p;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  deficiencyPath(type: number): string {
    return type === EDeficiencyType.Soil ? "soil" : "water";
  }

  deficiencyLinkCommands(type: number, id: string): string[] {
    const segment = this.deficiencyPath(type);
    const lang = this.languageService.getLanguageFromUrl();
    if (lang === "uk") return ["/", segment, id];
    return ["/", lang, segment, id];
  }
}
