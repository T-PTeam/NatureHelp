import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

import { environment } from "src/environments/environment.dev";

export interface IDeficiencyConfirmResponse {
  alreadyConfirmed: boolean;
  creatorRewarded: boolean;
}

@Injectable({
  providedIn: "root",
})
export class DeficiencyConfirmationService {
  private baseUrl = `${environment.apiUrl}/deficiency`;

  constructor(private http: HttpClient) {}

  confirm(deficiencyId: string, deficiencyType: number): Observable<IDeficiencyConfirmResponse> {
    return this.http.post<IDeficiencyConfirmResponse>(
      `${this.baseUrl}/${deficiencyId}/confirm?deficiencyType=${deficiencyType}`,
      {},
    );
  }
}
