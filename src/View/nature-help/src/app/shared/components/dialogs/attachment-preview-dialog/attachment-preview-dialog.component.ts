import { Component, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";

@Component({
  selector: "nat-attachment-preview-dialog",
  templateUrl: "./attachment-preview-dialog.component.html",
  styleUrls: ["./attachment-preview-dialog.component.css"],
  standalone: false,
})
export class AttachmentPreviewDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<AttachmentPreviewDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { url: string; fileName: string },
  ) {}

  close(): void {
    this.dialogRef.close();
  }
}
