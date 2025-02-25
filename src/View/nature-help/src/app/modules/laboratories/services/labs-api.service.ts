import { LoadingService } from '@/shared/services/loading.service';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BehaviorSubject, Observable, tap, catchError, shareReplay } from 'rxjs';
import { ILaboratory } from '../models/ILaboratory';

@Injectable({
  providedIn: 'root'
})
export class LabsAPIService {
private subject = new BehaviorSubject<ILaboratory[]>([]);
  public labs$: Observable<ILaboratory[]> = this.subject.asObservable();
  private labsUrl = 'https://localhost:7077/api/laboratory';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient,
    private notify: MatSnackBar,
    private loading: LoadingService){
      this.loadAllLabs();
    }

  public loadAllLabs(): Observable<ILaboratory[]> {
    const loadCourses$ = this.http.get<ILaboratory[]>(this.labsUrl)
      .pipe(
        tap(courses => this.subject.next(courses)),
        catchError(err => {
          const message = "Could not load labs";

          this.notify.open(message, 'Close', { duration: 2000 });
          return err;
        }),
        shareReplay()
    );
    this.loading.showLoaderUntilCompleted(loadCourses$).subscribe();
    return this.http.get<ILaboratory[]>(this.labsUrl);
  }

  public getLabById(id: string): Observable<ILaboratory> {
    return this.http.get<ILaboratory>(this.labsUrl + id.toString());
  }

  public addUserToLab(value: ILaboratory):Observable<ILaboratory>{
    return this.http.post<ILaboratory>(this.labsUrl, JSON.stringify(value), this.httpOptions);
  }

  public updateLabById(id: string, value: ILaboratory):Observable<ILaboratory>{
    return this.http.put<ILaboratory>(this.labsUrl + id, JSON.stringify(value), this.httpOptions);
  }

  public deleteLabById(id: string):Observable<any>{
    return this.http.delete<any>(this.labsUrl + id);
  }
}
