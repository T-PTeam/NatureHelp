import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable, shareReplay, Subscription, tap, throwError } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { IWaterDeficiency } from '@/modules/water-deficiency/models/IWaterDeficiency';
import { LoadingService } from '@/shared/services/loading.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable()
export class WaterAPIService {
  private subject = new BehaviorSubject<IWaterDeficiency[]>([]);
  public deficiencies$: Observable<IWaterDeficiency[]> = this.subject.asObservable();
  private watersUrl = 'https://localhost:7077/api/deficiency/water/';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient,
    private loading: LoadingService,
    private notify: MatSnackBar
  ) {
      this.loadAllWaterDeficiencies();
    }

  public loadAllWaterDeficiencies() {
    const loadCourses$ = this.http.get<IWaterDeficiency[]>(this.watersUrl)
      .pipe(
        tap(deficiencies => this.subject.next(deficiencies)),
        catchError(err => {
          const message = "Could not load water deficiencies";

          this.notify.open(message, 'Close', { duration: 2000 });
          return err;
        }),
        shareReplay()
    );
    this.loading.showLoaderUntilCompleted(loadCourses$).subscribe();
  }

  public getWaterDeficiencyById(id: string): Observable<IWaterDeficiency> {
    return this.http.get<IWaterDeficiency>(this.watersUrl + id.toString());
  }

  public addNewWaterDeficiency(value: IWaterDeficiency):Observable<IWaterDeficiency>{
    return this.http.post<IWaterDeficiency>(this.watersUrl, JSON.stringify(value), this.httpOptions);
  }

  public updateWaterDeficiencyById( // TODO
    id: string,
    changes: Partial<IWaterDeficiency>
  ): Observable<any> {
    const courses = this.subject.getValue();

    const index = courses.findIndex(course => course.id == id);

    const newCourse: IWaterDeficiency = {
      ...courses[index],
      ...changes
    };

    const newCourses: IWaterDeficiency[] = courses.slice(0);

    newCourses[index] = newCourse;

    this.subject.next(newCourses);

    return this.http
      .put(`api/courses/${id}`, changes)
      .pipe(
        catchError(err => {
          const message = "Could not update water deficiency";

          this.notify.open(message, 'Close', { duration: 2000 });

          return err;
        }),
        shareReplay())
  }
}
