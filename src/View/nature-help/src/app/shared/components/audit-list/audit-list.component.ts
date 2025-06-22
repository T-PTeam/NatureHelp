import { EDeficiencyType } from "@/models/enums";
import { IDeficiency } from "@/models/IDeficiency";
import { AuditService } from "@/shared/services/audit.service";
import { Component, Input } from "@angular/core";

@Component({
  selector: "nat-audit-list",
  templateUrl: "./audit-list.component.html",
  styleUrls: ["./audit-list.component.css"],
  standalone: false,
})
export class AuditListComponent {
  @Input() deficiency!: IDeficiency;
  displayedColumns: string[] = ["property", "old", "new"];

  private wasOpenedExpansionPanel: boolean = false;

  constructor(public auditService: AuditService) {}

  objectKeys = Object.keys;

  loadChangedModelHistory(id: string, type: EDeficiencyType) {
    if (!this.wasOpenedExpansionPanel) {
      this.auditService.loadDeficiencyHistory(id, type);
      this.wasOpenedExpansionPanel = true;
    }
  }

  getParsedChangesArray(json: string): { key: string; old: string; new: string }[] {
    try {
      const parsed = JSON.parse(json);
      return Object.keys(parsed).map((key) => ({
        key,
        old: parsed[key].Old,
        new: parsed[key].New,
      }));
    } catch {
      return [];
    }
  }
}
