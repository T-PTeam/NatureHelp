import { IAuthResponse } from '@/models/IAuthResponse';
import { IUser } from '@/models/IUser';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, map, tap, shareReplay } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'https://localhost:7077/api/user';

  private jwtHelper = new JwtHelperService();
  private subject = new BehaviorSubject<IUser | null>(null);

  $user: Observable<IUser | null> = this.subject.asObservable();
  isLoggedIn$: Observable<boolean>;
  isLoggedOut$: Observable<boolean>;

  constructor (private http: HttpClient){
    this.isLoggedIn$ = this.$user.pipe(map(user => !!user));
    this.isLoggedOut$ = this.$user.pipe(map(user => !user));

    this.setAuthOptions(null);
  }

  auth(isRegister: boolean, email: string, password: string): Observable<IAuthResponse> {
    return this.http.post<IAuthResponse>(`${this.apiUrl}/${isRegister ? 'register' : 'login'}`, {email, password})
      .pipe(
        tap(authResponse => this.setAuthOptions(authResponse)),
        shareReplay()
      )
  }

  logout(){
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('role');
    this.subject.next(null);
  }

  checkLocalAuthOptions(): void {
    const accessToken = localStorage.getItem('accessToken');
    const refreshToken = localStorage.getItem('refreshToken');
  
    if (!accessToken) {
      if (!refreshToken) {
        this.relogin();
        return;
      }
      this.refreshAccessToken(refreshToken);
      return;
    }
  
    this.http.post<boolean>(`${this.apiUrl}/check-token-expiration`, { token: accessToken })
      .pipe(
        tap(isExpired => {
          if (isExpired) {
            if (!refreshToken) {
              this.relogin(); 
            } else {
              this.refreshAccessToken(refreshToken);
            }
          }
        }),
        shareReplay()
      ).subscribe();
  }
  
  private refreshAccessToken(refreshToken: string): void {
    this.http.post<{ accessToken: string }>(`${this.apiUrl}/refresh-token`, { refreshToken })
      .pipe(
        tap(response => {
          if (response.accessToken) {
            localStorage.setItem('accessToken', response.accessToken);
          } else {
            this.relogin();
          }
        }),
        shareReplay()
      ).subscribe();
  }
  
  private setAuthOptions(authOptions: any){
    if (!authOptions){
      return;
    }

    if (authOptions.accessToken) localStorage.setItem('accessToken', authOptions.accessToken);
    if (authOptions.refreshToken) localStorage.setItem('refreshToken', authOptions.refreshToken);

    const decodedTokenRole = this.jwtHelper.decodeToken(authOptions.accessToken);
    console.log("TOKEN: ", this.jwtHelper.decodeToken(authOptions.accessToken))
    if (decodedTokenRole) localStorage.setItem('role', decodedTokenRole['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']);

    this.subject.next(authOptions);
  }

  private relogin(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('role');
    this.subject.next(null);
  }  
}
