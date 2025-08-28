import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment.dev";

@Injectable({
  providedIn: "root",
})
export class EmailVerificationService {
  private apiUrl = `${environment.apiUrl}/user`;

  constructor(private http: HttpClient) {}

  sendVerificationEmail(email: string): Observable<string> {
    return this.http.post(`${this.apiUrl}/send-verification-email`, { email }, { responseType: "text" });
  }

  confirmEmail(token: string): Observable<string> {
    return this.http.get(`${this.apiUrl}/confirm-email?token=${token}`, { responseType: "text" });
  }
}
