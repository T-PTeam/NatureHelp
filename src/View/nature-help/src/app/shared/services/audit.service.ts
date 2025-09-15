import { HttpHeaders, HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { BehaviorSubject, Observable, tap, catchError, of, shareReplay, map } from "rxjs";
import { LoadingService } from "./loading.service";
import { IChangedModelLog } from "@/models/IChangedModelLog";
import { EDeficiencyType } from "@/models/enums";
import { environment } from "src/environments/environment.dev";
import { WaterAPIService } from "@/modules/water-deficiency/services/water-api.service";
import { SoilAPIService } from "@/modules/soil-deficiency/services/soil-api.service";
import { IMonitoringScheme } from "@/models/IMonitoringScheme";

@Injectable({
  providedIn: "root",
})
export class AuditService {
  private deficiencyHistorySybject = new BehaviorSubject<IChangedModelLog[]>([]);
  private monitoringSoilDeficiencySubject = new BehaviorSubject<boolean>(false);
  private monitoringWaterDeficiencySubject = new BehaviorSubject<boolean>(false);

  public deficiencyHistory$: Observable<IChangedModelLog[]> = this.deficiencyHistorySybject.asObservable();
  public monitoringSoilDeficiency$: Observable<boolean> = this.monitoringSoilDeficiencySubject.asObservable();
  public monitoringWaterDeficiency$: Observable<boolean> = this.monitoringWaterDeficiencySubject.asObservable();

  private auditUrl = `${environment.apiUrl}/ChangedModelLogs`;
  private monitoringUrl = `${environment.apiUrl}/Monitoring`;

  httpOptions = {
    headers: new HttpHeaders({ "Content-Type": "application/json" }),
  };

  constructor(
    private http: HttpClient,
    private loading: LoadingService,
    private notify: MatSnackBar,
    private waterAPIService: WaterAPIService,
    private soilAPIService: SoilAPIService,
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

  public toggleMonitoring(deficiencyId: string | null, type: EDeficiencyType): Observable<boolean> {
    const deficiencyIdParam = deficiencyId ? `deficiencyId=${deficiencyId}` : "";
    const url = `${this.monitoringUrl}/toggle-monitoring?${deficiencyIdParam}&type=${type}`
      .replace(/&+/g, "&")
      .replace(/\?&/, "?");

    return this.http.put<boolean>(url, { refreshToken: localStorage.getItem("refreshToken") }, this.httpOptions).pipe(
      tap((response) => {
        if (!deficiencyId && response) {
          if (type === EDeficiencyType.Soil) {
            this.soilAPIService.loadSoilDeficiencies(0, null);
          } else {
            this.waterAPIService.loadWaterDeficiencies(0, null);
          }
        }

        if (type === EDeficiencyType.Soil) {
          this.monitoringSoilDeficiencySubject.next(response);
        } else {
          this.monitoringWaterDeficiencySubject.next(response);
        }
      }),
      map((success) => {
        if (success) {
          this.notify.open("Monitoring toggled successfully", "Close", { duration: 2000 });
        }
        return success;
      }),
      catchError((err) => {
        console.error("Error toggling monitoring:", err);
        throw err;
      }),
    );
  }

  public setMonitoringScheme(scheme: IMonitoringScheme): void {
    this.monitoringWaterDeficiencySubject.next(scheme.isMonitoringWaterDeficiencies);
    this.monitoringSoilDeficiencySubject.next(scheme.isMonitoringSoilDeficiencies);
  }
}
