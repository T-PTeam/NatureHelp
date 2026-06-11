import { IListData } from "@/shared/models/IListData";
import { appendSortParams } from "@/shared/helpers/table-sort.helper";
import { ITableSort } from "@/shared/models/ITableSort";
import { LoadingService } from "@/shared/services/loading.service";
import { HttpHeaders, HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { BehaviorSubject, Observable, tap, catchError, of, shareReplay } from "rxjs";
import { IResearch } from "../models/IResearch";
import { environment } from "src/environments/environment.dev";

@Injectable({
  providedIn: "root",
})
export class ResearchesAPIService {
  private researchesUrl = `${environment.apiUrl}/research`;

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

  public loadResearches(scrollCount: number, sort: ITableSort | null = null): Observable<IResearch[]> {
    let params = new HttpParams().set("scrollCount", scrollCount);
    params = appendSortParams(params, sort);

    const loadResearches$ = this.http.get<IListData<IResearch>>(this.researchesUrl, { params }).pipe(
      tap((listData) => {
        if (scrollCount === 0) {
          this.researchesSubject.next(listData.list);
        } else {
          this.researchesSubject.next([...this.researchesSubject.getValue(), ...listData.list]);
        }

        this.totalCountSubject.next(listData.totalCount);
      }),
      catchError(() => {
        this.notify.open("Could not load researches", "Close", { duration: 2000 });
        return of({ list: [], totalCount: 0 } as IListData<IResearch>);
      }),
      shareReplay(),
    );

    this.loading.showLoaderUntilCompleted(loadResearches$).subscribe();
    return this.researches$;
  }
}
