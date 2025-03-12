import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { BehaviorSubject, catchError, Observable, shareReplay, tap } from "rxjs";

import { ISoilDeficiency } from "@/modules/soil-deficiency/models/ISoilDeficiency";
import { LoadingService } from "@/shared/services/loading.service";
import { IListData } from "@/shared/models/IListData";

@Injectable()
export class SoilAPIService {
    private listSubject = new BehaviorSubject<ISoilDeficiency[]>([]);
    private totalCountSubject = new BehaviorSubject<number>(0);
    public deficiencies$: Observable<ISoilDeficiency[]> = this.listSubject.asObservable();
    public totalCount$: Observable<number> = this.totalCountSubject.asObservable();
    private soilsUrl = "https://localhost:7077/api/deficiency/soil/";

    httpOptions = {
        headers: new HttpHeaders({ "Content-Type": "application/json" }),
    };

    constructor(
        private http: HttpClient,
        private notify: MatSnackBar,
        private loading: LoadingService,
    ) {
        this.loadSoilDeficiencies(0);
    }

    public loadSoilDeficiencies(scrollCount: number): Observable<ISoilDeficiency[]> {
        const loadCourses$ = this.http
            .get<IListData<ISoilDeficiency>>(`${this.soilsUrl}?scrollCount=${scrollCount}`)
            .pipe(
                tap((listData) => {
                    this.listSubject.next([...this.listSubject.getValue(), ...listData.list]);
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
        return this.http.get<ISoilDeficiency[]>(this.soilsUrl);
    }

    public getSoilDeficiencyById(id: string): Observable<ISoilDeficiency> {
        return this.http.get<ISoilDeficiency>(this.soilsUrl + id.toString());
    }

    public addNewSoilDeficiency(value: ISoilDeficiency): Observable<ISoilDeficiency> {
        return this.http.post<ISoilDeficiency>(this.soilsUrl, JSON.stringify(value), this.httpOptions);
    }

    public updateSoilDeficiencyById(id: string, value: ISoilDeficiency): Observable<ISoilDeficiency> {
        return this.http.put<ISoilDeficiency>(this.soilsUrl + id, JSON.stringify(value), this.httpOptions);
    }

    public deleteSoilDeficiencyById(id: string): Observable<any> {
        return this.http.delete<any>(this.soilsUrl + id);
    }
}
