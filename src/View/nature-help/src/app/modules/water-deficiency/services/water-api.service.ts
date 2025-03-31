import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { BehaviorSubject, catchError, Observable, of, shareReplay, tap } from "rxjs";

import { IWaterDeficiency } from "@/modules/water-deficiency/models/IWaterDeficiency";
import { LoadingService } from "@/shared/services/loading.service";
import { IListData } from "@/shared/models/IListData";

@Injectable()
export class WaterAPIService {
  private listSubject = new BehaviorSubject<IWaterDeficiency[]>([]);
  private totalCountSubject = new BehaviorSubject<number>(0);
  public deficiencies$: Observable<IWaterDeficiency[]> = this.listSubject.asObservable();
  public totalCount$: Observable<number> = this.totalCountSubject.asObservable();
  private watersUrl = "https://localhost:7077/api/WaterDeficiency";

  httpOptions = {
    headers: new HttpHeaders({ "Content-Type": "application/json" }),
  };

  constructor(
    private http: HttpClient,
    private loading: LoadingService,
    private notify: MatSnackBar,
  ) {
    this.loadWaterDeficiencies(0);
  }

  public loadWaterDeficiencies(scrollCount: number) {
    const loadCourses$ = this.http
      .get<IListData<IWaterDeficiency>>(`${this.watersUrl}?scrollCount=${scrollCount}`)
      .pipe(
        tap((listData) => {
          this.listSubject.next([...this.listSubject.getValue(), ...listData.list]);
          this.totalCountSubject.next(listData.totalCount);
        }),
        catchError((err) => {
          const message = "Could not load water deficiencies";

          console.error(err);
          this.notify.open(message, "Close", { duration: 2000 });
          return of({ list: [], totalCount: 0 });
        }),
        shareReplay(),
      );
    this.loading.showLoaderUntilCompleted(loadCourses$).subscribe();
  }

  public getWaterDeficiencyById(id: string): Observable<IWaterDeficiency> {
    return this.http.get<IWaterDeficiency>(`${this.watersUrl}/${id.toString()}`);
  }

  public addNewWaterDeficiency(value: IWaterDeficiency): Observable<IWaterDeficiency> {
    return this.http.post<IWaterDeficiency>(this.watersUrl, JSON.stringify(value), this.httpOptions);
  }

  public updateWaterDeficiencyById(
    // TODO
    id: string,
    changes: Partial<IWaterDeficiency>,
  ): Observable<any> {
    const courses = this.listSubject.getValue();

    const index = courses.findIndex((course) => course.id == id);

    const newCourse: IWaterDeficiency = {
      ...courses[index],
      ...changes,
    };

    const newCourses: IWaterDeficiency[] = courses.slice(0);

    newCourses[index] = newCourse;

    this.listSubject.next(newCourses);

    return this.http.put(`${this.watersUrl}/${id}`, changes).pipe(
      catchError((err) => {
        const message = "Could not update water deficiency";

        this.notify.open(message, "Close", { duration: 2000 });

        return err;
      }),
      shareReplay(),
    );
  }
}
