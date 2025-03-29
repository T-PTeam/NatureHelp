import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { WaterAPIService } from "@/modules/water-deficiency/services/water-api.service";

import { withLatestFrom } from "rxjs";
import { ReportAPIService } from "@/shared/services/report-api.service";

@Component({
  selector: "n-water-deficiencys-deficiencies",
  templateUrl: "./water-deficiency-list.component.html",
  styleUrls: ["./water-deficiency-list.component.css"],
  standalone: false,
})
export class WaterDeficiencyList {
  public search: string = "";
  public scrollCheckDisabled: boolean = false;

  private listScrollCount = 0;

  constructor(
    public waterAPIService: WaterAPIService,
    private router: Router,
    private reportAPIService: ReportAPIService,
  ) {}

  public downloadExcel() {
    this.reportAPIService.downloadWaterDeficiencyExcelListFile();
  }

  public navigateToDetail(id?: string) {
    if (id) {
      this.router.navigate([`/water/${id}`]);
    } else {
      this.router.navigate(["water/add"]);
    }
  }

  onScroll() {
    this.listScrollCount++;
    this.waterAPIService.loadWaterDeficiencies(this.listScrollCount);

    this.waterAPIService.deficiencies$
      .pipe(withLatestFrom(this.waterAPIService.totalCount$))
      .subscribe(([deficiencies, totalCount]) => {
        this.scrollCheckDisabled = totalCount <= deficiencies.length;
      });
  }

  public goToSoil() {
    this.router.navigateByUrl("soil");
  }
}
