import { IUser } from '@/models/IUser';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, map, tap, shareReplay } from 'rxjs';

const AUTH_DATA = "auth_data";

@Injectable({
  providedIn: 'root'
})
export class UserService {

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

  login(email: string, password: string): Observable<IUser> {
    return this.http.post<IUser>("/api/login", {email, password})
      .pipe(
        tap(user => {
          this.subject.next(user);
          localStorage.setItem(AUTH_DATA, JSON.stringify(user));
        }),
        shareReplay()
      );
  }

  logout(){
    this.subject.next(null);
    localStorage.removeItem(AUTH_DATA);
  }
}
