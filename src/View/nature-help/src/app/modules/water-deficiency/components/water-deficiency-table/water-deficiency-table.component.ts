import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { WaterAPIService } from "@/modules/water-deficiency/services/water-api.service";

import { withLatestFrom } from "rxjs";
import { ReportAPIService } from "@/shared/services/report-api.service";
import { ILocation } from "@/models/ILocation";
import { MapViewService } from "@/shared/services/map-view.service";
import { FormBuilder, FormGroup } from "@angular/forms";
import { IWaterDeficiencyFilter } from "../../models/IWaterDeficiencyFilter";
import { enumToSelectOptions } from "@/shared/helpers/enum-helper";
import { EDangerState } from "@/models/enums";
import { ISelectOption } from "@/shared/models/ISelectOption";

@Component({
  selector: "n-water-deficiencies",
  templateUrl: "./water-deficiency-table.component.html",
  styleUrls: ["./water-deficiency-table.component.css", "../../../../shared/styles/table-list.component.css"],
  standalone: false,
})
export class WaterDeficiencyTable {
  search: string = "";
  scrollCheckDisabled: boolean = false;
  filterForm!: FormGroup;

  dangerStates: ISelectOption<EDangerState>[] = [];

  private listScrollCount = 0;
  private isFilterChanged: boolean = false;

  constructor(
    public waterAPIService: WaterAPIService,
    private router: Router,
    private reportAPIService: ReportAPIService,
    private mapViewService: MapViewService,
    private fb: FormBuilder,
  ) {
    this.dangerStates = enumToSelectOptions(EDangerState);

    this.filterForm = this.fb.group({
      title: [""],
      description: [""],
      type: [null],
      eDangerState: [null],
      changedModelLogId: [""],
    });

    this.filterForm.valueChanges.subscribe(() => {
      this.isFilterChanged = true;
    });
  }

  downloadExcel() {
    this.reportAPIService.downloadWaterDeficiencyExcelListFile();
  }

  navigateToDetail(id?: string) {
    if (id) {
      this.router.navigate([`/water/${id}`]);
    } else {
      this.router.navigate(["water/add"]);
    }
  }

  onScroll() {
    this.listScrollCount++;
    this.waterAPIService.loadWaterDeficiencies(this.listScrollCount, this.filterForm.value);

    this.waterAPIService.deficiencies$
      .pipe(withLatestFrom(this.waterAPIService.totalCount$))
      .subscribe(([deficiencies, totalCount]) => {
        this.scrollCheckDisabled = totalCount <= deficiencies.length;
      });
  }

  goToSoil() {
    this.router.navigateByUrl("soil");
  }

  changeMapFocus(location: ILocation) {
    this.mapViewService.changeFocus(location, 20);
  }

  applyFilter(): void {
    const filter: IWaterDeficiencyFilter = this.filterForm.value;

    if (this.isFilterChanged) this.listScrollCount = 0;

    this.waterAPIService.loadWaterDeficiencies(this.listScrollCount, filter);
  }
}
