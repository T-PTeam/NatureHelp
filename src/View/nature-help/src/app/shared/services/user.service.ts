import { IAuthResponse } from '@/models/IAuthResponse';
import { IUser } from '@/models/IUser';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, map, tap, shareReplay } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';

const AUTH_DATA = "auth_data";

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

    this.isLoggedOut$ = this.$user.pipe(map(user => !this.isLoggedIn$));

    const user = localStorage.getItem(AUTH_DATA);

    if (user){
      this.subject.next(JSON.parse(user));
    }
  }

  login(email: string, password: string): Observable<IAuthResponse> {
    return this.http.post<IAuthResponse>(`${this.apiUrl}/login`, {email, password})
      .pipe(
        tap(authResponse => {
          localStorage.setItem('token', authResponse.accessToken);
          const decodedToken = this.jwtHelper.decodeToken(authResponse.accessToken);
          localStorage.setItem('role', decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']);
          console.log("TOKEN, DECODED: ", authResponse.accessToken, decodedToken);

          this.subject.next(authResponse.user);
          localStorage.setItem(AUTH_DATA, JSON.stringify(authResponse));
        }),
        shareReplay()
      );
  }  

  logout(){
    this.subject.next(null);
    localStorage.removeItem(AUTH_DATA);
  }
}
