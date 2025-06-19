import { HttpHandler, HttpInterceptor, HttpRequest, HttpErrorResponse, HttpEvent } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { catchError, switchMap } from "rxjs/operators";
import { UserAPIService } from "./user-api.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private userService: UserAPIService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = sessionStorage.getItem("accessToken");
    let authReq = req;
    if (token) {
      authReq = req.clone({
        setHeaders: { Authorization: `Bearer ${token}` },
      });
    }

    return next.handle(authReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401 && localStorage.getItem("refreshToken")) {
          return this.userService.refreshAccessToken().pipe(
            switchMap(() => {
              const newToken = sessionStorage.getItem("accessToken");
              const retryReq = req.clone({
                setHeaders: { Authorization: `Bearer ${newToken}` },
              });
              return next.handle(retryReq);
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
