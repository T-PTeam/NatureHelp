import { IListData } from '@/shared/models/IListData';
import { LoadingService } from '@/shared/services/loading.service';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BehaviorSubject, Observable, tap, catchError, shareReplay } from 'rxjs';
import { IResearch } from '../models/IResearch';

@Injectable({
  providedIn: 'root'
})
export class ResearchesAPIService {
  private researchesUrl = "https://localhost:7077/api/research";

  private researchesSubject = new BehaviorSubject<IResearch[]>([]);
  public researches$: Observable<IResearch[]> = this.researchesSubject.asObservable();

  private totalCountSubject = new BehaviorSubject<number>(0);
  public totalCount$: Observable<number> = this.totalCountSubject.asObservable();

  httpOptions = {
    headers: new HttpHeaders({ "Content-Type": "application/json" }),
  };

  constructor(
    private http: HttpClient,
    private notify: MatSnackBar,
    private loading: LoadingService,
  ) {
    this.loadResearches(0);
  }

  public loadResearches(scrollCount: number): Observable<IResearch[]> {
    const loadCourses$ = this.http.get<IListData<IResearch>>(`${this.researchesUrl}?scrollCount=${scrollCount}`).pipe(
      tap((listData) => {
        this.researchesSubject.next([...this.researchesSubject.getValue(), ...listData.list]);
        this.totalCountSubject.next(listData.totalCount);
      }),
      catchError((err) => {
        const message = "Could not load soil deficiencies";

        this.notify.open(message, "Close", { duration: 2000 });
        return err;
      }),
      shareReplay(),
    );
    
    this.loading.showLoaderUntilCompleted(loadCourses$).subscribe();
    return this.http.get<IResearch[]>(`${this.researchesUrl}?scrollCount=${scrollCount}`);
  }
}
