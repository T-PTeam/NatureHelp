import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";
import { BehaviorSubject, catchError, map, Observable, of, shareReplay, tap } from "rxjs";

import { IAuthResponse } from "@/models/IAuthResponse";
import { IUser } from "@/models/IUser";
import { IListData } from "../models/IListData";
import { MatSnackBar } from "@angular/material/snack-bar";
import { LoadingService } from "./loading.service";

@Injectable({
    providedIn: "root",
})
export class UserAPIService {
    private apiUrl = "https://localhost:7077/api/user";

    private jwtHelper = new JwtHelperService();
    private subject = new BehaviorSubject<IUser | null>(null);

    $user: Observable<IUser | null> = this.subject.asObservable();
    isLoggedIn$: Observable<boolean>;
    isLoggedOut$: Observable<boolean>;

    private organizationUsersSubject = new BehaviorSubject<IUser[]>([]);
    $organizationUsers: Observable<IUser[]> = this.organizationUsersSubject.asObservable();
    
    private totalCountSubject = new BehaviorSubject<number>(0);
    $totalCount: Observable<number> = this.totalCountSubject.asObservable();

    constructor(
        private http: HttpClient,
        private loading: LoadingService,
        private notify: MatSnackBar

    ) {
        this.isLoggedIn$ = this.$user.pipe(map((user) => !!user));
        this.isLoggedOut$ = this.$user.pipe(map((user) => !user));

        this.setAuthOptions(null);
        this.loadOrganizationUsers(0);
    }

    auth(isRegister: boolean, email: string, password: string): Observable<IAuthResponse> {
        return this.http
            .post<IAuthResponse>(`${this.apiUrl}/${isRegister ? "register" : "login"}`, { email, password })
            .pipe(
                tap((authResponse) => this.setAuthOptions(authResponse)),
                shareReplay(),
            );
    }

    logout() {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        localStorage.removeItem("role");
        this.subject.next(null);
    }

    loadOrganizationUsers(scrollCount: number) {
        const organizationId = localStorage.getItem("organizationId");

        if (!organizationId) {
            this.notify.open("Relogin, please", "Close", { duration: 2000 });
            return;
        }

        if (scrollCount === -1){
            this.organizationUsersSubject.next([]);
        }

        const loadOrganizationUsers$ = this.http
            .get<IListData<IUser>>(`${this.apiUrl}/organization-users?organizationId=${organizationId}&scrollCount=${scrollCount}`)
            .pipe(
                tap((listData) => {
                    this.organizationUsersSubject.next([...this.organizationUsersSubject.getValue(), ...listData.list]);
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

    changeUsersRoles(changedUsersRoles: Map<string, number>){
        const payload = Object.fromEntries(changedUsersRoles);

        console.log("changeUsersRoles changedUsersRoles", changedUsersRoles)
        const updateOrganizationUsersRoles$ = this.http.put<boolean>(`${this.apiUrl}/users-roles`, payload)
        .pipe(
            tap((updateResult) => {
                const message = updateResult
                    ? "Users' roles were successfully changed!"
                    : "Error occured by the updating users' roles...";

                this.notify.open(message, "Close", { duration: 2000 })
                this.loadOrganizationUsers(-1);
            }),
            shareReplay(),
            catchError(err => {
                this.notify.open("Error: " + err, "Close", { duration: 2000 })
                this.loadOrganizationUsers(-1)

                return of(false); 
            })
        );

        this.loading.showLoaderUntilCompleted(updateOrganizationUsersRoles$).subscribe();
    }

    checkLocalAuthOptions(): void {
        const accessToken = localStorage.getItem("accessToken");
        const refreshToken = localStorage.getItem("refreshToken");

        if (!accessToken) {
            if (!refreshToken) {
                this.relogin();
                return;
            }
            this.refreshAccessToken(refreshToken);
            return;
        }

        this.http
            .post<boolean>(`${this.apiUrl}/check-token-expiration`, { token: accessToken })
            .pipe(
                tap((isExpired) => {
                    if (isExpired) {
                        if (!refreshToken) {
                            this.relogin();
                        } else {
                            this.refreshAccessToken(refreshToken);
                        }
                    }
                }),
                shareReplay(),
            )
            .subscribe();
    }

    private refreshAccessToken(refreshToken: string): void {
        this.http
            .post<{ accessToken: string }>(`${this.apiUrl}/refresh-token`, { refreshToken })
            .pipe(
                tap((response) => {
                    if (response.accessToken) {
                        localStorage.setItem("accessToken", response.accessToken);
                    } else {
                        this.relogin();
                    }
                }),
                shareReplay(),
            )
            .subscribe();
    }

    private setAuthOptions(authOptions: any) {
        if (!authOptions) {
            return;
        }

        if (authOptions.accessToken) localStorage.setItem("accessToken", authOptions.accessToken);
        if (authOptions.refreshToken) localStorage.setItem("refreshToken", authOptions.refreshToken);
        if (authOptions.organizationId) localStorage.setItem("organizationId", authOptions.organizationId);

        const decodedTokenRole = this.jwtHelper.decodeToken(authOptions.accessToken);
        if (decodedTokenRole)
            localStorage.setItem(
                "role",
                decodedTokenRole["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
            );

        this.subject.next(authOptions);
    }

    private relogin(): void {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        localStorage.removeItem("role");
        this.subject.next(null);
    }
}
