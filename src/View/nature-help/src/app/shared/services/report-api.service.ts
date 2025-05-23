import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { saveAs } from "file-saver";
import { LoadingService } from "@/shared/services/loading.service";
import { environment } from "src/environments/environment.dev";

@Injectable()
export class ReportAPIService {
  private reportsUrl = `${environment}apiUrl/api/report`;

  httpOptions = {
    headers: new HttpHeaders({ "Content-Type": "application/json" }),
  };

  constructor(
    private http: HttpClient,
    private loading: LoadingService,
    private notify: MatSnackBar,
  ) {}

  public downloadWaterDeficiencyExcelListFile(): any {
    this.http
      .get(this.reportsUrl + "water", { responseType: "blob" })
      .subscribe((fileBlob) => saveAs(fileBlob, "Water Deficiencies"));
  }

  public downloadSoilDeficiencyExcelListFile(): any {
    return this.http
      .get(this.reportsUrl + "soil", { responseType: "blob" })
      .subscribe((fileBlob) => saveAs(fileBlob, "Soil Deficiencies"));
  }

  public downloadOrgUsersExcelListFile(): any {
    return this.http
      .get(this.reportsUrl + "org-users", { responseType: "blob" })
      .subscribe((fileBlob) => saveAs(fileBlob, "Organization Users"));
  }
}
