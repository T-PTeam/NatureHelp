import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { SoilAPIService } from "@/modules/soil-deficiency/services/soil-api.service";
import { withLatestFrom } from "rxjs";
import { ReportAPIService } from "@/shared/services/report-api.service";

@Component({
    selector: "n-soil-deficiencys-deficiencies",
    templateUrl: "./soil-deficiency-list.component.html",
    styleUrls: ["./soil-deficiency-list.component.css"],
    standalone: false,
})
export class SoilDeficiencyList {
    public search: string = "";
    public scrollCheckDisabled: boolean = false;

    private listScrollCount = 0;

    constructor(
        public soilAPIService: SoilAPIService,
        private router: Router,
        private reportAPIService: ReportAPIService
    ) {}

    public downloadExcel() {
        this.reportAPIService.downloadSoilDeficiencyExcelListFile();
    }

    public navigateToDetail(id?: string) {
        if (id) {
            this.router.navigate([`/soil/${id}`]);
        } else {
            this.router.navigate([`soil/add`]);
        }
    }

    onScroll() {
        this.listScrollCount++;
        this.soilAPIService.loadSoilDeficiencies(this.listScrollCount);

        this.soilAPIService.deficiencies$
            .pipe(withLatestFrom(this.soilAPIService.totalCount$))
            .subscribe(([deficiencies, totalCount]) => {
                this.scrollCheckDisabled = totalCount <= deficiencies.length;
            });
    }

    public goToWater() {
        this.router.navigateByUrl("/water");
    }
}
