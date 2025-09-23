import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, catchError, of, shareReplay, tap } from "rxjs";

import { IOrganization } from "@/models/IOrganization";
import { IListData } from "@/shared/models/IListData";
import { LoadingService } from "@/shared/services/loading.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { environment } from "src/environments/environment.dev";

@Injectable({
  providedIn: "root",
})
export class OrganizationAPIService {
  private apiUrl = `${environment.apiUrl}/organization`;

  private listSubject = new BehaviorSubject<IOrganization[]>([]);
  public organizations$: Observable<IOrganization[]> = this.listSubject.asObservable();

  private totalCountSubject = new BehaviorSubject<number>(0);
  public totalCount$: Observable<number> = this.totalCountSubject.asObservable();

  constructor(
    private http: HttpClient,
    private loading: LoadingService,
    private notify: MatSnackBar,
  ) {
    this.loadOrganizations(-1);
  }

  loadOrganizations(scrollCount: number): Observable<IOrganization[]> {
    let params = new HttpParams();

    if (scrollCount !== undefined && scrollCount !== null) {
      params = params.set("scrollCount", scrollCount.toString());
    }

    const loadOrganizations$ = this.http.get<IListData<IOrganization>>(`${this.apiUrl}`, { params }).pipe(
      tap((listData) => {
        if (scrollCount === -1) {
          console.log("API Service: Setting organizations list (scrollCount=-1):", listData.list);
          this.listSubject.next(listData.list);
        } else if (scrollCount === 0) {
          console.log("API Service: Setting organizations list (scrollCount=0):", listData.list);
          this.listSubject.next(listData.list);
        } else {
          console.log("API Service: Appending to organizations list:", listData.list);
          this.listSubject.next([...this.listSubject.getValue(), ...listData.list]);
        }
        this.totalCountSubject.next(listData.totalCount);
      }),
      catchError((err) => {
        const message = "Could not load organizations";
        console.error(err);
        this.notify.open(message, "Close", { duration: 2000 });
        return of({ list: [], totalCount: 0 } as IListData<IOrganization>);
      }),
      shareReplay(),
    );

    this.loading.showLoaderUntilCompleted(loadOrganizations$).subscribe();
    return this.organizations$;
  }

  getOrganizationById(id: string): Observable<IOrganization> {
    return this.http.get<IOrganization>(`${this.apiUrl}/${id}`);
  }

  createOrganization(organization: IOrganization): Observable<IOrganization> {
    return this.http.post<IOrganization>(`${this.apiUrl}`, organization);
  }

  updateOrganization(id: string, organization: IOrganization): Observable<IOrganization> {
    return this.http.put<IOrganization>(`${this.apiUrl}/${id}`, organization);
  }

  deleteOrganization(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
