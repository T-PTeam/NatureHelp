import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { BehaviorSubject, catchError, concatMap, from, Observable, of, shareReplay, tap } from "rxjs";

import { ISoilDeficiency } from "@/modules/soil-deficiency/models/ISoilDeficiency";
import { IDeficiencyMapDto } from "@/models/IDeficiencyMapDto";
import { appendSortParams } from "@/shared/helpers/table-sort.helper";
import { ITableSort } from "@/shared/models/ITableSort";
import { LoadingService } from "@/shared/services/loading.service";
import { IListData } from "@/shared/models/IListData";
import { ISoilDeficiencyFilter } from "../models/ISoilDeficiencyFilter";
import { environment } from "src/environments/environment.dev";

@Injectable()
export class SoilAPIService {
  private listSubject = new BehaviorSubject<ISoilDeficiency[]>([]);
  private totalCountSubject = new BehaviorSubject<number>(0);
  private mapDataSubject = new BehaviorSubject<IDeficiencyMapDto[]>([]);
  public deficiencies$: Observable<ISoilDeficiency[]> = this.listSubject.asObservable();
  public totalCount$: Observable<number> = this.totalCountSubject.asObservable();
  public mapDeficiencies$: Observable<IDeficiencyMapDto[]> = this.mapDataSubject.asObservable();
  private soilsUrl = `${environment.apiUrl}/soildeficiency`;
  private lastScrollCount = 0;
  private lastFilter: ISoilDeficiencyFilter | null = null;
  private lastSort: ITableSort | null = null;

  httpOptions = {
    headers: new HttpHeaders({ "Content-Type": "application/json" }),
  };

  constructor(
    private http: HttpClient,
    private notify: MatSnackBar,
    private loading: LoadingService,
  ) {}

  public loadSoilDeficiencies(
    scrollCount: number,
    filter: ISoilDeficiencyFilter | null,
    sort: ITableSort | null = null,
  ): Observable<ISoilDeficiency[]> {
    this.lastScrollCount = scrollCount;
    this.lastFilter = filter ? { ...filter } : null;
    this.lastSort = sort ? { ...sort } : null;

    const loadDeficiencies$ = this.fetchSoilDeficiencies(scrollCount, filter, sort);
    this.loading.showLoaderUntilCompleted(loadDeficiencies$).subscribe();
    return this.deficiencies$;
  }

  public reloadCurrentData(): void {
    const pages = Array.from({ length: this.lastScrollCount + 1 }, (_, index) => index);
    const reload$ = from(pages).pipe(
      concatMap((page) => this.fetchSoilDeficiencies(page, this.lastFilter, this.lastSort)),
      shareReplay(),
    );

    this.loading.showLoaderUntilCompleted(reload$).subscribe();
    this.loadAllSoilDeficienciesForMap();
  }

  public loadAllSoilDeficienciesForMap() {
    this.http
      .get<IListData<IDeficiencyMapDto>>(`${this.soilsUrl}/map-objects`)
      .pipe(
        tap((data) => {
          this.mapDataSubject.next(data.list);
        }),
        catchError((err) => {
          console.error("Could not load soil deficiencies for map", err);
          return of({ list: [], totalCount: 0 } as IListData<IDeficiencyMapDto>);
        }),
        shareReplay(),
      )
      .subscribe();
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

  private fetchSoilDeficiencies(
    scrollCount: number,
    filter: ISoilDeficiencyFilter | null,
    sort: ITableSort | null,
  ): Observable<IListData<ISoilDeficiency>> {
    let params = new HttpParams();

    if (scrollCount || scrollCount === 0) params = params.set("scrollCount", scrollCount);
    if (filter?.title) params = params.set("Title", filter.title);
    if (filter?.description) params = params.set("Description", filter.description);
    if (filter?.eDangerState || filter?.eDangerState === 0)
      params = params.set("EDangerState", filter.eDangerState.toString());
    params = appendSortParams(params, sort);

    return this.http.get<IListData<ISoilDeficiency>>(`${this.soilsUrl}`, { params }).pipe(
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
  }
}
