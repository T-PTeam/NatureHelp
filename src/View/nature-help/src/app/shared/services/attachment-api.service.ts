import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../environments/environment.dev";
import { IDeficiencyAttachment } from "@/models/IAttachment";
import { EDeficiencyType } from "@/models/enums";

@Injectable({
  providedIn: "root",
})
export class AttachmentAPIService {
  private readonly baseUrl = `${environment.apiUrl}/attachments`;

  constructor(private http: HttpClient) {}

  /**
   * Get all attachments for a specific deficiency
   * @param deficiencyId - The ID of the deficiency
   * @returns Observable of attachment array
   */
  getAttachments(deficiencyId: string, deficiencyType: EDeficiencyType): Observable<IDeficiencyAttachment[]> {
    return this.http.get<IDeficiencyAttachment[]>(
      `${this.baseUrl}/deficiency/${deficiencyId}?deficiencyType=${deficiencyType}`,
    );
  }

  /**
   * Get a specific attachment by ID
   * @param attachmentId - The ID of the attachment
   * @returns Observable of attachment
   */
  getAttachment(attachmentId: string): Observable<IDeficiencyAttachment> {
    return this.http.get<IDeficiencyAttachment>(`${this.baseUrl}/${attachmentId}`);
  }

  /**
   * Upload a new attachment for a deficiency
   * @param deficiencyId - The ID of the deficiency
   * @param file - The file to upload
   * @returns Observable of the created attachment
   */
  uploadAttachment(deficiencyId: string, file: File): Observable<IDeficiencyAttachment> {
    const formData = new FormData();
    formData.append("file", file);
    formData.append("deficiencyId", deficiencyId);

    return this.http.post<IDeficiencyAttachment>(`${this.baseUrl}/upload`, formData);
  }

  /**
   * Delete an attachment
   * @param attachmentId - The ID of the attachment to delete
   * @returns Observable of void
   */
  deleteAttachment(attachmentId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${attachmentId}`);
  }

  /**
   * Get the download URL for an attachment
   * @param attachmentId - The ID of the attachment
   * @returns Observable of the download URL
   */
  getDownloadUrl(attachmentId: string): Observable<{ downloadUrl: string }> {
    return this.http.get<{ downloadUrl: string }>(`${this.baseUrl}/${attachmentId}/download`);
  }

  downloadAttachmentBlob(attachmentId: string): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/${attachmentId}/download`, { responseType: "blob" });
  }
}
