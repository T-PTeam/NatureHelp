import { Component, Input } from "@angular/core";
import { CommonModule } from "@angular/common";
import { TranslateModule } from "@ngx-translate/core";
import { MatCardModule } from "@angular/material/card";
import { MatIconModule } from "@angular/material/icon";

import { DynamicAvatarComponent } from "@/shared/components/dynamic-avatar/dynamic-avatar.component";
import { IProfileStats } from "@/models/profile/IProfileStats";

@Component({
  selector: "nat-profile-overview-tab",
  standalone: true,
  imports: [CommonModule, TranslateModule, MatCardModule, MatIconModule, DynamicAvatarComponent],
  templateUrl: "./profile-overview-tab.component.html",
  styleUrls: ["./profile-overview-tab.component.css"],
})
export class ProfileOverviewTabComponent {
  @Input() stats: IProfileStats | null = null;
}
