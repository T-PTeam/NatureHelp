import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, Observable, shareReplay, Subscription, tap, throwError } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { ISoilDeficiency } from '@/modules/soil-deficiency/models/ISoilDeficiency';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LoadingService } from '@/shared/services/loading.service';

@Injectable()
export class SoilAPIService {
  private subject = new BehaviorSubject<ISoilDeficiency[]>([]);
  public deficiencies$: Observable<ISoilDeficiency[]> = this.subject.asObservable();
  private soilsUrl = 'https://localhost:7077/api/deficiency/soil/';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient,
    private notify: MatSnackBar,
    private loading: LoadingService){
      this.loadAllSoilDeficiencies();
    }

  public loadAllSoilDeficiencies(): Observable<ISoilDeficiency[]> {
    const loadCourses$ = this.http.get<ISoilDeficiency[]>(this.soilsUrl)
      .pipe(
        tap(courses => this.subject.next(courses)),
        catchError(err => {
          const message = "Could not load soil deficiencies";

          this.notify.open(message, 'Close', { duration: 2000 });
          return err;
        }),
        shareReplay()
    );
    this.loading.showLoaderUntilCompleted(loadCourses$).subscribe();
    return this.http.get<ISoilDeficiency[]>(this.soilsUrl);
  }

  public getSoilDeficiencyById(id: string): Observable<ISoilDeficiency> {
    return this.http.get<ISoilDeficiency>(this.soilsUrl + id.toString());
  }

  public addNewSoilDeficiency(value: ISoilDeficiency):Observable<ISoilDeficiency>{
    return this.http.post<ISoilDeficiency>(this.soilsUrl, JSON.stringify(value), this.httpOptions);
  }

  public updateSoilDeficiencyById(id: string, value: ISoilDeficiency):Observable<ISoilDeficiency>{
    return this.http.put<ISoilDeficiency>(this.soilsUrl + id, JSON.stringify(value), this.httpOptions);
  }

  public deleteSoilDeficiencyById(id: string):Observable<any>{
    return this.http.delete<any>(this.soilsUrl + id);
  }
}
