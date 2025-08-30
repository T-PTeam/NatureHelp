import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../environments/environment.dev";
import { ICommentMessage } from "@/models/ICommentMessage";
import { EDeficiencyType } from "@/models/enums";

@Injectable({
  providedIn: "root",
})
export class CommentAPIService {
  private readonly baseUrl = `${environment.apiUrl}/CommentMessages`;

  constructor(private http: HttpClient) {}

  /**
   * Get all comments for a specific deficiency
   * @param deficiencyId - The ID of the deficiency
   * @returns Observable of comment array
   */
  getComments(deficiencyId: string, deficiencyType: EDeficiencyType): Observable<ICommentMessage[]> {
    return this.http.get<ICommentMessage[]>(
      `${this.baseUrl}/deficiency/${deficiencyId}?deficiencyType=${deficiencyType}`,
    );
  }

  /**
   * Get a specific comment by ID
   * @param commentId - The ID of the comment
   * @returns Observable of comment
   */
  getComment(commentId: string): Observable<ICommentMessage> {
    return this.http.get<ICommentMessage>(`${this.baseUrl}/${commentId}`);
  }

  /**
   * Add a new comment message for a deficiency
   */
  addComment(comment: {
    message: string;
    deficiencyId: string;
    deficiencyType: EDeficiencyType;
    creatorFullName: string;
  }): Observable<ICommentMessage> {
    return this.http.post<ICommentMessage>(`${this.baseUrl}`, comment);
  }

  /**
   * Upload a new comment for a deficiency
   * @param deficiencyId - The ID of the deficiency
   * @param file - The file to upload
   * @returns Observable of the created comment
   */
  uploadComment(deficiencyId: string, file: File): Observable<ICommentMessage> {
    const formData = new FormData();
    formData.append("file", file);
    formData.append("deficiencyId", deficiencyId);

    return this.http.post<ICommentMessage>(`${this.baseUrl}/upload`, formData);
  }

  /**
   * Delete an comment
   * @param commentId - The ID of the comment to delete
   * @returns Observable of void
   */
  deleteComment(commentId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${commentId}`);
  }
}
