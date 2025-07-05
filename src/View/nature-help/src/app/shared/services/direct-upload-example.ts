import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../environments/environment";
import { IDeficiencyAttachment } from "@/models/IAttachment";

@Injectable({
  providedIn: "root",
})
export class DirectUploadExample {
  constructor(private http: HttpClient) {}

  uploadFileDirect(file: File, deficiencyId: string): Observable<IDeficiencyAttachment> {
    const formData = new FormData();
    formData.append("file", file);
    formData.append("deficiencyId", deficiencyId);

    return this.http.post<IDeficiencyAttachment>(`${environment.apiUrl}/attachments/upload`, formData);
  }

  uploadWithCustomHeaders(file: File, deficiencyId: string, customHeaders: any): Observable<IDeficiencyAttachment> {
    const formData = new FormData();
    formData.append("file", file);
    formData.append("deficiencyId", deficiencyId);

    return this.http.post<IDeficiencyAttachment>(`${environment.apiUrl}/attachments/upload`, formData, {
      headers: customHeaders,
    });
  }

  uploadWithProgressTracking(file: File, deficiencyId: string): Observable<any> {
    const formData = new FormData();
    formData.append("file", file);
    formData.append("deficiencyId", deficiencyId);

    return this.http.post(`${environment.apiUrl}/attachments/upload`, formData, {
      reportProgress: true,
      observe: "events",
    });
  }
}
