import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";
import { BehaviorSubject, catchError, map, Observable, of, shareReplay, tap } from "rxjs";

import { IAuthResponse } from "@/models/IAuthResponse";
import { IUser } from "@/models/IUser";
import { IListData } from "../models/IListData";
import { MatSnackBar } from "@angular/material/snack-bar";
import { LoadingService } from "./loading.service";
import { EAuthType } from "@/models/enums";
import { environment } from "src/environments/environment.dev";

@Injectable({
  providedIn: "root",
})
export class UserAPIService {
  private apiUrl = `${environment.apiUrl}/user`;

  private jwtHelper = new JwtHelperService();
  private subject = new BehaviorSubject<IUser | null>(null);

  $user: Observable<IUser | null> = this.subject.asObservable();
  isLoggedIn$: Observable<boolean>;
  isLoggedOut$: Observable<boolean>;

  isSuperAdmin$: Observable<boolean>;
  isOwner$: Observable<boolean>;

  private organizationUsersSubject = new BehaviorSubject<IUser[]>([]);
  $organizationUsers: Observable<IUser[]> = this.organizationUsersSubject.asObservable();

  private notLoginEverOrganizationUsersSubject = new BehaviorSubject<IUser[]>([]);
  $notLoginEverOrganizationUsers: Observable<IUser[]> = this.notLoginEverOrganizationUsersSubject.asObservable();

  private totalCountSubject = new BehaviorSubject<number>(0);
  $totalCount: Observable<number> = this.totalCountSubject.asObservable();

  constructor(
    private http: HttpClient,
    private loading: LoadingService,
    private notify: MatSnackBar,
  ) {
    this.isLoggedIn$ = this.$user.pipe(map((user) => !!user));
    this.isLoggedOut$ = this.$user.pipe(map((user) => !user));

    this.isSuperAdmin$ = this.$user.pipe(map((user) => user?.role === 0));
    this.isOwner$ = this.$user.pipe(map((user) => user?.role === 1));
  }

  auth(authType: EAuthType, email: string, password: string | null): Observable<IAuthResponse> {
    if (password) {
      return this.http
        .post<IAuthResponse>(`${this.apiUrl}/${authType}`, {
          email,
          password,
          organizationId: sessionStorage.getItem("organizationId"),
        })
        .pipe(
          tap((authResponse) => {
            this.logout();
            this.setAuthOptions(authResponse);
          }),
          shareReplay(),
        );
    } else {
      return this.http
        .post<IAuthResponse>(`${this.apiUrl}/${authType}`, {
          email,
          organizationId: sessionStorage.getItem("organizationId"),
        })
        .pipe(
          tap((authResponse) => {
            this.logout();
            this.setAuthOptions(authResponse);
          }),
          shareReplay(),
        );
    }
  }

  refreshAccessToken(): Observable<IAuthResponse> {
    return this.http
      .post<IAuthResponse>(`${this.apiUrl}/refresh-access-token`, {
        refreshToken: localStorage.getItem("refreshToken"),
      })
      .pipe(
        tap((authResponse) => {
          this.logout();
          this.setAuthOptions(authResponse);
        }),
        shareReplay(),
      );
  }

  logout() {
    sessionStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    sessionStorage.removeItem("role");
    sessionStorage.removeItem("organizationId");
    sessionStorage.removeItem("email");
    sessionStorage.removeItem("fullName");
    sessionStorage.removeItem("userId");

    this.subject.next(null);
  }

  loadOrganizationUsers(scrollCount: number) {
    const organizationId = sessionStorage.getItem("organizationId");

    if (!organizationId) {
      this.notify.open("Relogin, please", "Close", { duration: 2000 });
      return;
    }

    if (scrollCount === -1) {
      this.organizationUsersSubject.next([]);
    }

    const loadOrganizationUsers$ = this.http
      .get<
        IListData<IUser>
      >(`${this.apiUrl}/organization-users?organizationId=${organizationId}&scrollCount=${scrollCount}`)
      .pipe(
        tap((listData) => {
          if (scrollCount === 0) this.organizationUsersSubject.next(listData.list);
          else this.organizationUsersSubject.next([...this.organizationUsersSubject.getValue(), ...listData.list]);

          this.totalCountSubject.next(listData.totalCount);
        }),
        catchError((err) => {
          const message = "Could not load organization users...";

          console.error(err);
          this.notify.open(message, "Close", { duration: 2000 });
          return of({ list: [], totalCount: 0 });
        }),
        shareReplay(),
      );
    this.loading.showLoaderUntilCompleted(loadOrganizationUsers$).subscribe();
  }

  loadNotLoginEverOrganizationUsers() {
    const organizationId = sessionStorage.getItem("organizationId");

    if (!organizationId) {
      this.notify.open("Relogin, please", "Close", { duration: 2000 });
      return;
    }

    const loadNotLoginEverOrganizationUsers$ = this.http
      .get<IListData<IUser>>(`${this.apiUrl}/users-not-login-ever?organizationId=${organizationId}`)
      .pipe(
        tap((listData) => {
          this.totalCountSubject.next(0);
          this.notLoginEverOrganizationUsersSubject.next(listData.list);
        }),
        catchError((err) => {
          const message = "Could not load organization users...";

          console.error(err);
          this.notify.open(message, "Close", { duration: 2000 });
          return of({ list: [], totalCount: 0 });
        }),
        shareReplay(),
      );
    this.loading.showLoaderUntilCompleted(loadNotLoginEverOrganizationUsers$).subscribe();
  }

  addOrganizationUsers(authType: EAuthType, users: IUser[]) {
    const organizationId = sessionStorage.getItem("organizationId");

    users = users.map((u) => {
      u.organizationId = organizationId;
      return u;
    });

    return this.http.post<IUser[]>(`${this.apiUrl}/${authType}`, users).pipe(
      tap((users) =>
        this.notify.open(`Users (${users.map((u) => u.email).join(", ")}) were added to Your organization`, "Close", {
          duration: 10000,
        }),
      ),
      shareReplay(),
    );
  }

  addOrganizationUser(authType: EAuthType, user: IUser) {
    const organizationId = sessionStorage.getItem("organizationId");

    user.organizationId = organizationId;

    return this.http.post<IUser>(`${this.apiUrl}/${authType}`, user).pipe(
      tap((user) =>
        this.notify.open(`User (${user.email}) was added to Your organization`, "Close", {
          duration: 6000,
        }),
      ),
      shareReplay(),
    );
  }

  changeUsersRoles(changedUsersRoles: Map<string, number>) {
    const payload = Object.fromEntries(changedUsersRoles);

    const updateOrganizationUsersRoles$ = this.http.put<boolean>(`${this.apiUrl}/users-roles`, payload).pipe(
      tap((updateResult) => {
        const message = updateResult
          ? "Users' roles were successfully changed!"
          : "Error occured by the updating users' roles...";

        this.notify.open(message, "Close", { duration: 2000 });
        this.loadOrganizationUsers(-1);
      }),
      shareReplay(),
      catchError((err) => {
        this.notify.open("Error: " + err, "Close", { duration: 2000 });
        this.loadOrganizationUsers(-1);

        return of(false);
      }),
    );

    this.loading.showLoaderUntilCompleted(updateOrganizationUsersRoles$).subscribe();
  }

  resetPassword(newPassword: string, token: string): Observable<boolean> {
    return this.http
      .post<boolean>(`${this.apiUrl}/reset-password`, {
        token: token,
        newPassword: newPassword,
      })
      .pipe(
        tap((success) => {
          const message = success ? "Password was successfully reset!" : "Error occurred while resetting password...";

          this.notify.open(message, "Close", { duration: 2000 });
        }),
        shareReplay(),
        catchError((err) => {
          this.notify.open("Error: " + err, "Close", { duration: 2000 });
          return of(false);
        }),
      );
  }

  sendPasswordResetLink(email: string): Observable<boolean> {
    return this.http
      .post<boolean>(`${this.apiUrl}/send-password-reset-link`, {
        email: email,
      })
      .pipe(
        tap((success) => {
          const message = success
            ? "Password reset link has been sent to your email!"
            : "Error occurred while sending password reset link...";

          this.notify.open(message, "Close", { duration: 2000 });
        }),
        shareReplay(),
        catchError((err) => {
          this.notify.open("Error: " + err, "Close", { duration: 2000 });
          return of(false);
        }),
      );
  }

  private setAuthOptions(authOptions: any) {
    if (!authOptions) {
      return;
    }

    if (authOptions.id) sessionStorage.setItem("userId", authOptions.id);
    if (authOptions.accessToken) sessionStorage.setItem("accessToken", authOptions.accessToken);
    if (authOptions.refreshToken) localStorage.setItem("refreshToken", authOptions.refreshToken);
    if (authOptions.organizationId) sessionStorage.setItem("organizationId", authOptions.organizationId);
    if (authOptions.email) sessionStorage.setItem("email", authOptions.email);
    if (authOptions.firstName && authOptions.lastName)
      sessionStorage.setItem("fullName", `${authOptions.firstName} ${authOptions.lastName}`);

    const decodedTokenRole = this.jwtHelper.decodeToken(authOptions.accessToken);
    if (decodedTokenRole)
      sessionStorage.setItem("role", decodedTokenRole["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]);

    this.subject.next(authOptions);
  }
}
