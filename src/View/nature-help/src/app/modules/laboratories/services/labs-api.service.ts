import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { BehaviorSubject, catchError, Observable, shareReplay, tap } from "rxjs";

import { LoadingService } from "@/shared/services/loading.service";

import { ILaboratory } from "../models/ILaboratory";
import { IListData } from "@/shared/models/IListData";

@Injectable({
  providedIn: "root",
})
export class LabsAPIService {
  private labsUrl = "https://localhost:7077/api/laboratory";

  private labsSubject = new BehaviorSubject<ILaboratory[]>([]);
  public labs$: Observable<ILaboratory[]> = this.labsSubject.asObservable();

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
    this.loadLabs(0);
  }

  public loadLabs(scrollCount: number): Observable<ILaboratory[]> {
    const loadlabs$ = this.http.get<IListData<ILaboratory>>(`${this.labsUrl}?scrollCount=${scrollCount}`).pipe(
      tap((listData) => {
        this.labsSubject.next([...this.labsSubject.getValue(), ...listData.list]);
        this.totalCountSubject.next(listData.totalCount);
      }),
      catchError((err) => {
        const message = "Could not load labs";

        this.notify.open(message, "Close", { duration: 2000 });
        return err;
      }),
      shareReplay(),
    );

    this.loading.showLoaderUntilCompleted(loadlabs$).subscribe();
    return this.http.get<ILaboratory[]>(`${this.labsUrl}?scrollCount=${scrollCount}`);
  }

  public getLabById(id: string): Observable<ILaboratory> {
    return this.http.get<ILaboratory>(this.labsUrl + id.toString());
  }

  public addUserToLab(value: ILaboratory): Observable<ILaboratory> {
    return this.http.post<ILaboratory>(this.labsUrl, JSON.stringify(value), this.httpOptions);
  }

  public updateLabById(id: string, value: ILaboratory): Observable<ILaboratory> {
    return this.http.put<ILaboratory>(this.labsUrl + id, JSON.stringify(value), this.httpOptions);
  }

  public deleteLabById(id: string): Observable<any> {
    return this.http.delete<any>(this.labsUrl + id);
  }
}
