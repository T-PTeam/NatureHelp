import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { BehaviorSubject, catchError, Observable, of, shareReplay, tap } from "rxjs";

import { LoadingService } from "@/shared/services/loading.service";

import { ILaboratory } from "../models/ILaboratory";
import { ILaboratoryMapDto } from "../models/ILaboratoryMapDto";
import { IListData } from "@/shared/models/IListData";
import { ILaboratorFilter } from "../models/ILaboratoryFilter";
import { environment } from "src/environments/environment.dev";

@Injectable({
  providedIn: "root",
})
export class LabsAPIService {
  private labsUrl = `${environment.apiUrl}/laboratory`;

  private labsSubject = new BehaviorSubject<ILaboratory[]>([]);
  public labs$: Observable<ILaboratory[]> = this.labsSubject.asObservable();

  private mapDataSubject = new BehaviorSubject<ILaboratoryMapDto[]>([]);
  public mapLabs$: Observable<ILaboratoryMapDto[]> = this.mapDataSubject.asObservable();

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
    this.loadLabs(0, null);
    this.loadAllLabsForMap();
  }

  public loadLabs(scrollCount: number, filter: ILaboratorFilter | null): Observable<ILaboratory[]> {
    let params = new HttpParams();

    if (scrollCount || scrollCount === 0) params = params.set("scrollCount", scrollCount);
    if (filter?.title) params = params.set("Title", filter.title);

    const loadlabs$ = this.http.get<IListData<ILaboratory>>(`${this.labsUrl}`, { params }).pipe(
      tap((listData) => {
        if (scrollCount === 0) this.labsSubject.next(listData.list);
        else this.labsSubject.next([...this.labsSubject.getValue(), ...listData.list]);

        this.totalCountSubject.next(listData.totalCount);
      }),
      catchError((err) => {
        const message = "Could not load labs";

        this.notify.open(message, "Close", { duration: 2000 });
        return of({ list: [], totalCount: 0 } as IListData<ILaboratory>);
      }),
      shareReplay(),
    );

    this.loading.showLoaderUntilCompleted(loadlabs$).subscribe();
    return this.labs$;
  }

  public loadAllLabsForMap() {
    this.http
      .get<IListData<ILaboratoryMapDto>>(`${this.labsUrl}/map-objects`)
      .pipe(
        tap((data) => {
          this.mapDataSubject.next(data.list);
        }),
        catchError((err) => {
          console.error("Could not load labs for map", err);
          return of({ list: [], totalCount: 0 } as IListData<ILaboratoryMapDto>);
        }),
        shareReplay(),
      )
      .subscribe();
  }

  public getLabById(id: string): Observable<ILaboratory> {
    return this.http.get<ILaboratory>(`${this.labsUrl}/${id}`);
  }

  public addUserToLab(value: ILaboratory): Observable<ILaboratory> {
    return this.http.post<ILaboratory>(this.labsUrl, JSON.stringify(value), this.httpOptions);
  }

  public addLab(value: ILaboratory): Observable<ILaboratory> {
    return this.http.post<ILaboratory>(`${this.labsUrl}`, JSON.stringify(value), this.httpOptions);
  }

  public updateLabById(id: string, value: ILaboratory): Observable<ILaboratory> {
    return this.http.put<ILaboratory>(`${this.labsUrl}/${id}`, JSON.stringify(value), this.httpOptions);
  }

  public deleteLabById(id: string): Observable<any> {
    return this.http.delete<any>(`${this.labsUrl}/${id}`);
  }
}
