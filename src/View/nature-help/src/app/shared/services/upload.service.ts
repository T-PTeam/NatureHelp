import { Injectable } from "@angular/core";
import { HttpClient, HttpEvent, HttpEventType, HttpErrorResponse } from "@angular/common/http";
import { Observable, BehaviorSubject, throwError } from "rxjs";
import { map, catchError, tap } from "rxjs/operators";
import { environment } from "../../../environments/environment.dev";
import { IDeficiencyAttachment } from "@/models/IAttachment";

export interface UploadProgress {
  file: File;
  progress: number;
  status: "pending" | "uploading" | "completed" | "error";
  error?: string;
}

export interface UploadResult {
  file: File;
  attachment?: IDeficiencyAttachment;
  error?: string;
}

@Injectable({
  providedIn: "root",
})
export class UploadService {
  private uploadProgressSubject = new BehaviorSubject<UploadProgress[]>([]);
  public uploadProgress$ = this.uploadProgressSubject.asObservable();

  constructor(private http: HttpClient) {}

  /**
   * Upload a single file with progress tracking
   */
  uploadFile(file: File, deficiencyId: string): Observable<IDeficiencyAttachment> {
    console.log("Starting upload for file:", file.name, "deficiencyId:", deficiencyId);

    const formData = new FormData();
    formData.append("file", file);
    formData.append("deficiencyId", deficiencyId);

    this.updateProgress(file, 0, "uploading");

    const uploadUrl = `${environment.apiUrl}/attachments/upload`;
    console.log("Upload URL:", uploadUrl);

    return this.http
      .post<IDeficiencyAttachment>(uploadUrl, formData, {
        reportProgress: true,
        observe: "events",
      })
      .pipe(
        map((event: HttpEvent<any>) => {
          console.log("Upload event:", event.type, event);
          switch (event.type) {
            case HttpEventType.UploadProgress:
              const progress = Math.round((100 * event.loaded) / (event.total || file.size));
              this.updateProgress(file, progress, "uploading");
              return null;
            case HttpEventType.Response:
              this.updateProgress(file, 100, "completed");
              console.log("Upload completed successfully:", event.body);
              return event.body;
            default:
              return null;
          }
        }),
        catchError((error: HttpErrorResponse) => {
          console.error("Upload error:", error);
          const errorMessage = this.getErrorMessage(error);
          this.updateProgress(file, 0, "error", errorMessage);
          return throwError(() => errorMessage);
        }),
      );
  }

  /**
   * Upload multiple files sequentially
   */
  uploadFiles(files: File[], deficiencyId: string): Observable<UploadResult[]> {
    const results: UploadResult[] = [];
    let completedCount = 0;

    return new Observable((observer) => {
      const uploadNext = (index: number) => {
        if (index >= files.length) {
          observer.next(results);
          observer.complete();
          return;
        }

        const file = files[index];
        this.uploadFile(file, deficiencyId).subscribe({
          next: (attachment) => {
            results.push({ file, attachment });
            completedCount++;
            observer.next(results);
            uploadNext(index + 1);
          },
          error: (error) => {
            results.push({ file, error });
            completedCount++;
            observer.next(results);
            uploadNext(index + 1);
          },
        });
      };

      uploadNext(0);
    });
  }

  /**
   * Upload multiple files in parallel (with concurrency limit)
   */
  uploadFilesParallel(files: File[], deficiencyId: string, concurrency: number = 3): Observable<UploadResult[]> {
    const results: UploadResult[] = [];
    let completedCount = 0;
    let currentIndex = 0;

    return new Observable((observer) => {
      const startUpload = (index: number) => {
        if (index >= files.length) return;

        const file = files[index];
        this.uploadFile(file, deficiencyId).subscribe({
          next: (attachment) => {
            results[index] = { file, attachment };
            completedCount++;
            observer.next(results);

            if (completedCount === files.length) {
              observer.complete();
            } else if (currentIndex < files.length) {
              startUpload(currentIndex++);
            }
          },
          error: (error) => {
            results[index] = { file, error };
            completedCount++;
            observer.next(results);

            if (completedCount === files.length) {
              observer.complete();
            } else if (currentIndex < files.length) {
              startUpload(currentIndex++);
            }
          },
        });
      };

      for (let i = 0; i < Math.min(concurrency, files.length); i++) {
        startUpload(currentIndex++);
      }
    });
  }

  /**
   * Retry upload for a failed file
   */
  retryUpload(file: File, deficiencyId: string): Observable<IDeficiencyAttachment> {
    return this.uploadFile(file, deficiencyId);
  }

  /**
   * Cancel upload (not implemented in this basic version)
   * In a real implementation, you would use AbortController
   */
  cancelUpload(file: File): void {
    this.updateProgress(file, 0, "error", "Upload cancelled");
  }

  /**
   * Clear all upload progress
   */
  clearProgress(): void {
    this.uploadProgressSubject.next([]);
  }

  /**
   * Get current upload progress for a specific file
   */
  getFileProgress(file: File): UploadProgress | undefined {
    return this.uploadProgressSubject.value.find((p) => p.file === file);
  }

  /**
   * Get overall upload progress
   */
  getOverallProgress(): number {
    const progress = this.uploadProgressSubject.value;
    if (progress.length === 0) return 0;

    const totalProgress = progress.reduce((sum, p) => sum + p.progress, 0);
    return Math.round(totalProgress / progress.length);
  }

  /**
   * Check if any uploads are in progress
   */
  hasActiveUploads(): boolean {
    return this.uploadProgressSubject.value.some((p) => p.status === "uploading");
  }

  private updateProgress(file: File, progress: number, status: UploadProgress["status"], error?: string): void {
    const currentProgress = this.uploadProgressSubject.value;
    const existingIndex = currentProgress.findIndex((p) => p.file === file);

    const updatedProgress: UploadProgress = { file, progress, status, error };

    if (existingIndex >= 0) {
      currentProgress[existingIndex] = updatedProgress;
    } else {
      currentProgress.push(updatedProgress);
    }

    this.uploadProgressSubject.next([...currentProgress]);
  }

  private getErrorMessage(error: HttpErrorResponse): string {
    if (error.error instanceof ErrorEvent) {
      return `Network error: ${error.error.message}`;
    } else {
      switch (error.status) {
        case 400:
          return "Invalid file format or size";
        case 401:
          return "Unauthorized. Please log in again";
        case 403:
          return "Access denied";
        case 413:
          return "File too large";
        case 500:
          return "Server error. Please try again later";
        default:
          return `Upload failed (${error.status}): ${error.message}`;
      }
    }
  }
}
