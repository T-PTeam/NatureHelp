import { Component, Inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";

import { IUser } from "@/models/IUser";

interface ILabResearchersDialogData {
  researchers: IUser[];
  selectedResearcherIds: string[];
}

@Component({
  selector: "nat-lab-researchers-dialog",
  templateUrl: "./lab-researchers-dialog.component.html",
  styleUrls: ["./lab-researchers-dialog.component.css"],
  standalone: false,
})
export class LabResearchersDialogComponent {
  selectedResearcherIds = new Set<string>();

  constructor(
    private dialogRef: MatDialogRef<LabResearchersDialogComponent, string[]>,
    @Inject(MAT_DIALOG_DATA) public data: ILabResearchersDialogData,
  ) {
    data.selectedResearcherIds.forEach((id) => this.selectedResearcherIds.add(id));
  }

  toggleResearcher(researcherId: string, checked: boolean): void {
    if (checked) {
      this.selectedResearcherIds.add(researcherId);
      return;
    }

    this.selectedResearcherIds.delete(researcherId);
  }

  isSelected(researcherId: string): boolean {
    return this.selectedResearcherIds.has(researcherId);
  }

  getLaboratoriesText(researcher: IUser): string {
    return researcher.laboratories?.map((laboratory) => laboratory.title).join(", ") ?? "";
  }

  save(): void {
    this.dialogRef.close(Array.from(this.selectedResearcherIds));
  }

  cancel(): void {
    this.dialogRef.close();
  }
}
