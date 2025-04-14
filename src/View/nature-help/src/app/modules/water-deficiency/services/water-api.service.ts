import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { BehaviorSubject, catchError, Observable, of, shareReplay, tap } from "rxjs";

import { IWaterDeficiency } from "@/modules/water-deficiency/models/IWaterDeficiency";
import { LoadingService } from "@/shared/services/loading.service";
import { IListData } from "@/shared/models/IListData";
import { IWaterDeficiencyFilter } from "../models/IWaterDeficiencyFilter";

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
    this.loadWaterDeficiencies(0, null);
  }

  public loadWaterDeficiencies(scrollCount: number, filter: IWaterDeficiencyFilter | null) {
    let params = new HttpParams();

    if (scrollCount || scrollCount === 0) params = params.set("scrollCount", scrollCount);
    if (filter?.title) params = params.set("Title", filter.title);
    if (filter?.description) params = params.set("Description", filter.description);
    if (filter?.eDangerState) params = params.set("EDangerState", filter.eDangerState.toString());

    const loaddeficiencies$ = this.http.get<IListData<IWaterDeficiency>>(`${this.watersUrl}`, { params }).pipe(
      tap((listData) => {
        if (scrollCount === 0) this.listSubject.next(listData.list);
        else this.listSubject.next([...this.listSubject.getValue(), ...listData.list]);

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
    this.loading.showLoaderUntilCompleted(loaddeficiencies$).subscribe();
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
    const deficiencies = this.listSubject.getValue();

    const index = deficiencies.findIndex((deficiency) => deficiency.id == id);

    const newDeficiency: IWaterDeficiency = {
      ...deficiencies[index],
      ...changes,
    };

    const newDeficiencies: IWaterDeficiency[] = deficiencies.slice(0);

    newDeficiencies[index] = newDeficiency;

    this.listSubject.next(newDeficiencies);

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
