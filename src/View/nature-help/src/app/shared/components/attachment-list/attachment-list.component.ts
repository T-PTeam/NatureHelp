import { IDeficiency } from "@/models/IDeficiency";
import { IDeficiencyAttachment } from "@/models/IAttachment";
import { Component, Input } from "@angular/core";
import { AttachmentAPIService } from "@/shared/services/attachment-api.service";
import { EDeficiencyType } from "@/models/enums";
import { MatDialog } from "@angular/material/dialog";
import { AttachmentPreviewDialogComponent } from "../dialogs/attachment-preview-dialog/attachment-preview-dialog.component";

@Component({
  selector: "nat-attachment-list",
  templateUrl: "./attachment-list.component.html",
  styleUrls: ["./attachment-list.component.css"],
  standalone: false,
})
export class AttachmentListComponent {
  @Input() deficiency!: IDeficiency;
  @Input() attachments: IDeficiencyAttachment[] = [];

  displayedColumns: string[] = ["fileName", "fileSize", "mimeType", "actions"];

  constructor(
    private attachmentService: AttachmentAPIService,
    private dialog: MatDialog,
  ) {}

  formatFileSize(bytes: number): string {
    if (bytes === 0) return "0 Bytes";
    const k = 1024;
    const sizes = ["Bytes", "KB", "MB", "GB"];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + " " + sizes[i];
  }

  downloadAttachment(attachment: IDeficiencyAttachment): void {
    this.attachmentService.downloadAttachmentBlob(attachment.id).subscribe((blob) => {
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement("a");
      link.href = url;
      link.download = attachment.fileName;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
    });
  }

  openAttachment(attachment: IDeficiencyAttachment): void {
    this.dialog.open(AttachmentPreviewDialogComponent, {
      data: {
        url: attachment.previewUrl,
        fileName: attachment.fileName,
      },
      panelClass: "attachment-preview-modal",
      maxWidth: "90vw",
    });
  }

  loadAttachments(deficiencyId: string, deficiencyType: EDeficiencyType) {
    if (!deficiencyId) {
      console.warn("No deficiency ID provided for loading attachments");
      return;
    }

    this.attachmentService.getAttachments(deficiencyId, deficiencyType).subscribe({
      next: (attachments) => {
        this.attachments = attachments || [];
      },
      error: (error) => {
        console.error("Error loading attachments:", error);
        this.attachments = [];
      },
    });
  }

  isImageFile(mimeType: string): boolean {
    return mimeType.startsWith("image/");
  }

  isPdfFile(mimeType: string): boolean {
    return mimeType === "application/pdf";
  }
}
