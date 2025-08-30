import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { BehaviorSubject, catchError, Observable, of, shareReplay, tap } from "rxjs";

import { ISoilDeficiency } from "@/modules/soil-deficiency/models/ISoilDeficiency";
import { LoadingService } from "@/shared/services/loading.service";
import { IListData } from "@/shared/models/IListData";
import { ISoilDeficiencyFilter } from "../models/ISoilDeficiencyFilter";
import { environment } from "src/environments/environment.dev";

@Injectable()
export class SoilAPIService {
  private listSubject = new BehaviorSubject<ISoilDeficiency[]>([]);
  private totalCountSubject = new BehaviorSubject<number>(0);
  public deficiencies$: Observable<ISoilDeficiency[]> = this.listSubject.asObservable();
  public totalCount$: Observable<number> = this.totalCountSubject.asObservable();
  private soilsUrl = `${environment.apiUrl}/SoilDeficiency`;

  httpOptions = {
    headers: new HttpHeaders({ "Content-Type": "application/json" }),
  };

  constructor(
    private http: HttpClient,
    private notify: MatSnackBar,
    private loading: LoadingService,
  ) {
    this.loadSoilDeficiencies(0, null);
  }

  public loadSoilDeficiencies(
    scrollCount: number,
    filter: ISoilDeficiencyFilter | null,
  ): Observable<ISoilDeficiency[]> {
    let params = new HttpParams();

    if (scrollCount || scrollCount === 0) params = params.set("scrollCount", scrollCount);
    if (filter?.title) params = params.set("Title", filter.title);
    if (filter?.description) params = params.set("Description", filter.description);
    if (filter?.eDangerState || filter?.eDangerState === 0)
      params = params.set("EDangerState", filter.eDangerState.toString());

    const loadDeficiencies$ = this.http.get<IListData<ISoilDeficiency>>(`${this.soilsUrl}`, { params }).pipe(
      tap((listData) => {
        if (scrollCount === 0) this.listSubject.next(listData.list);
        else this.listSubject.next([...this.listSubject.getValue(), ...listData.list]);

        this.totalCountSubject.next(listData.totalCount);
      }),
      catchError((err) => {
        const message = "Could not load soil deficiencies";

        this.notify.open(message, "Close", { duration: 2000 });
        return of({ list: [], totalCount: 0 } as IListData<ISoilDeficiency>);
      }),
      shareReplay(),
    );
    this.loading.showLoaderUntilCompleted(loadDeficiencies$).subscribe();
    return this.deficiencies$;
  }

  public getSoilDeficiencyById(id: string): Observable<ISoilDeficiency> {
    return this.http.get<ISoilDeficiency>(`${this.soilsUrl}/${id}`);
  }

  public addNewSoilDeficiency(value: ISoilDeficiency): Observable<ISoilDeficiency> {
    return this.http.post<ISoilDeficiency>(this.soilsUrl, JSON.stringify(value), this.httpOptions);
  }

  public updateSoilDeficiencyById(id: string, value: ISoilDeficiency): Observable<ISoilDeficiency> {
    return this.http.put<ISoilDeficiency>(`${this.soilsUrl}/${id}`, JSON.stringify(value), this.httpOptions);
  }

  public deleteSoilDeficiencyById(id: string): Observable<any> {
    return this.http.delete<any>(this.soilsUrl + id);
  }
}
