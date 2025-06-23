import { Component, EventEmitter, Input, Output } from "@angular/core";
import { IDeficiencyAttachment } from "@/models/IAttachment";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment.dev";
import { EDeficiencyType } from "@/models/enums";
import { MatSnackBar } from "@angular/material/snack-bar";

@Component({
  selector: "nat-file-upload",
  templateUrl: "./file-upload.component.html",
  styleUrls: ["./file-upload.component.css"],
  standalone: false,
})
export class FileUploadComponent {
  @Input() deficiencyId!: string;
  @Input() deficiencyType!: EDeficiencyType;

  @Output() fileUploaded = new EventEmitter<void>();

  selectedFile: File | null = null;
  baseUrl: string = environment.apiUrl + "/Attachments";

  private allowedTypes = ["image/png", "image/jpeg", "application/pdf"];
  private maxSize = 4 * 1024 * 1024;

  constructor(
    private http: HttpClient,
    private notify: MatSnackBar,
  ) {}

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      const file = input.files?.[0];

      if (file.size > this.maxSize) {
        alert("File size exceeds 10 MB.");
        return;
      }

      if (!this.allowedTypes.includes(file.type)) {
        this.notify.open("Only PNG, JPG, and PDF files are allowed.", "Close", { duration: 2000 });
        return;
      }

      this.selectedFile = input.files[0];
    }
  }

  uploadFile() {
    if (!this.selectedFile || !this.deficiencyId || this.deficiencyType === undefined) return;

    const formData = new FormData();
    formData.append("file", this.selectedFile);

    this.http
      .post(`${this.baseUrl}/upload/deficiency/${this.deficiencyId}?deficiencyType=${this.deficiencyType}`, formData)
      .subscribe({
        next: () => {
          this.selectedFile = null;
          this.fileUploaded.emit();
        },
        error: (err) => {
          this.notify.open("Upload failed!", "Close", { duration: 2000 });
          console.error("Upload failed", err);
        },
      });
  }
}
