import { HttpHandler, HttpInterceptor, HttpRequest, HttpErrorResponse, HttpEvent } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { catchError, switchMap } from "rxjs/operators";
import { UserAPIService } from "./user-api.service";

const AUTH_PATH_SEGMENTS = [
  "login",
  "register",
  "refresh-access-token",
  "current-user",
  "logout",
  "send-password-reset-link",
  "reset-password",
];

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private userService: UserAPIService) {}

  private isAuthRequest(req: HttpRequest<unknown>): boolean {
    const url = req.url;
    if (!url.includes("/user/")) return false;
    const afterUser = url.split("/user/")[1]?.split("?")[0]?.split("/")[0] ?? "";
    return AUTH_PATH_SEGMENTS.includes(afterUser);
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = sessionStorage.getItem("accessToken");
    const requestOptions: { setHeaders?: { Authorization: string }; withCredentials?: boolean } = {};
    if (token) {
      requestOptions.setHeaders = { Authorization: `Bearer ${token}` };
    }
    if (this.isAuthRequest(req)) {
      requestOptions.withCredentials = true;
    }
    const authReq = req.clone(requestOptions);

    return next.handle(authReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401 && localStorage.getItem("refreshToken")) {
          return this.userService.refreshAccessToken().pipe(
            switchMap(() => {
              const newToken = sessionStorage.getItem("accessToken");
              const retryOptions: { setHeaders: { Authorization: string }; withCredentials?: boolean } = {
                setHeaders: { Authorization: `Bearer ${newToken}` },
              };
              if (this.isAuthRequest(req)) {
                retryOptions.withCredentials = true;
              }
              return next.handle(req.clone(retryOptions));
            }),
            catchError((err) => {
              this.userService.logout();
              return throwError(() => err);
            }),
          );
        }
        return throwError(() => error);
      }),
    );
  }
}
