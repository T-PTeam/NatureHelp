import { HttpHeaders, HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { BehaviorSubject, Observable, tap, catchError, of, shareReplay } from "rxjs";
import { LoadingService } from "./loading.service";
import { IChangedModelLog } from "@/models/IChangedModelLog";
import { EDeficiencyType } from "@/models/enums";
import { environment } from "src/environments/environment.dev";

@Injectable({
  providedIn: "root",
})
export class AuditService {
  private deficiencyHistorySybject = new BehaviorSubject<IChangedModelLog[]>([]);
  public deficiencyHistory$: Observable<IChangedModelLog[]> = this.deficiencyHistorySybject.asObservable();
  private auditUrl = `${environment.apiUrl}/ChangedModelLogs`;

  httpOptions = {
    headers: new HttpHeaders({ "Content-Type": "application/json" }),
  };

  constructor(
    private http: HttpClient,
    private loading: LoadingService,
    private notify: MatSnackBar,
  ) {}

  public loadDeficiencyHistory(id: string, type: EDeficiencyType = EDeficiencyType.Water) {
    const loaddeficiencies$ = this.http
      .get<IChangedModelLog[]>(`${this.auditUrl}/deficiency-history?deficiencyId=${id}&type=${type}`)
      .pipe(
        tap((list) => {
          this.deficiencyHistorySybject.next(list);
        }),
        catchError((err) => {
          const message = "Could not load deficiency history";

          console.error(err);
          this.notify.open(message, "Close", { duration: 2000 });
          return of([]);
        }),
        shareReplay(),
      );
    this.loading.showLoaderUntilCompleted(loaddeficiencies$).subscribe();
  }
}
