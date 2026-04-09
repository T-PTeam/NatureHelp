import { Component, Input } from "@angular/core";
import { CommonModule } from "@angular/common";
import { TranslateModule } from "@ngx-translate/core";
import { AvatarStage } from "@/models/profile/avatar-stage.enum";

@Component({
  selector: "nat-dynamic-avatar",
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: "./dynamic-avatar.component.html",
  styleUrls: ["./dynamic-avatar.component.css"],
})
export class DynamicAvatarComponent {
  @Input() stage: AvatarStage | string = AvatarStage.Seed;
  @Input() size: "sm" | "md" | "lg" = "md";
  @Input() showLabel = true;

  readonly AvatarStage = AvatarStage;

  get stageClass(): string {
    const s = typeof this.stage === "string" ? this.stage : this.stage;
    return `avatar-stage-${s}`;
  }
}
