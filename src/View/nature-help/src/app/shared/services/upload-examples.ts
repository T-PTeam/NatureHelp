import { Injectable } from "@angular/core";
import { UploadService } from "./upload.service";
import { IDeficiencyAttachment } from "@/models/IAttachment";

@Injectable({
  providedIn: "root",
})
export class UploadExamples {
  constructor(private uploadService: UploadService) {}

  /**
   * Example 1: Upload a single file
   */
  uploadSingleFile(file: File, deficiencyId: string) {
    this.uploadService.uploadFile(file, deficiencyId).subscribe({
      next: (attachment: IDeficiencyAttachment) => {
        console.log("File uploaded successfully:", attachment);
        // Handle success - e.g., add to attachments list
      },
      error: (error) => {
        console.error("Upload failed:", error);
        // Handle error
      },
    });
  }

  /**
   * Example 2: Upload multiple files sequentially
   */
  uploadMultipleFilesSequentially(files: File[], deficiencyId: string) {
    this.uploadService.uploadFiles(files, deficiencyId).subscribe({
      next: (results) => {
        console.log("Upload results:", results);
        // results contains array of { file, attachment?, error? }
        const successfulUploads = results.filter((r) => r.attachment);
        const failedUploads = results.filter((r) => r.error);

        console.log(`Successfully uploaded: ${successfulUploads.length} files`);
        console.log(`Failed uploads: ${failedUploads.length} files`);
      },
      error: (error) => {
        console.error("Upload failed:", error);
      },
    });
  }

  /**
   * Example 3: Upload multiple files in parallel
   */
  uploadMultipleFilesParallel(files: File[], deficiencyId: string, concurrency: number = 3) {
    this.uploadService.uploadFilesParallel(files, deficiencyId, concurrency).subscribe({
      next: (results) => {
        console.log("Parallel upload results:", results);
        // Handle results
      },
      error: (error) => {
        console.error("Parallel upload failed:", error);
      },
    });
  }

  /**
   * Example 4: Upload with progress tracking
   */
  uploadWithProgress(file: File, deficiencyId: string) {
    // Subscribe to progress updates
    this.uploadService.uploadProgress$.subscribe((progress) => {
      const fileProgress = progress.find((p) => p.file === file);
      if (fileProgress) {
        console.log(`File ${file.name}: ${fileProgress.progress}% - ${fileProgress.status}`);
      }
    });

    // Start upload
    this.uploadService.uploadFile(file, deficiencyId).subscribe({
      next: (attachment) => {
        console.log("Upload completed:", attachment);
      },
      error: (error) => {
        console.error("Upload failed:", error);
      },
    });
  }

  /**
   * Example 5: Retry failed upload
   */
  retryFailedUpload(file: File, deficiencyId: string) {
    this.uploadService.retryUpload(file, deficiencyId).subscribe({
      next: (attachment) => {
        console.log("Retry successful:", attachment);
      },
      error: (error) => {
        console.error("Retry failed:", error);
      },
    });
  }

  /**
   * Example 6: Handle file input change event
   */
  handleFileInputChange(event: any, deficiencyId: string) {
    const files: FileList = event.target.files;
    if (files.length > 0) {
      const fileArray = Array.from(files);
      this.uploadMultipleFilesParallel(fileArray, deficiencyId);
    }
  }

  /**
   * Example 7: Upload from drag and drop
   */
  handleDragAndDrop(event: DragEvent, deficiencyId: string) {
    event.preventDefault();
    const files: FileList | null = event.dataTransfer?.files || null;

    if (files && files.length > 0) {
      const fileArray = Array.from(files);
      this.uploadMultipleFilesParallel(fileArray, deficiencyId);
    }
  }
}
