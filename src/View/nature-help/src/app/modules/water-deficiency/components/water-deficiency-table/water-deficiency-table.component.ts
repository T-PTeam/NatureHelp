import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";

import { WaterAPIService } from "@/modules/water-deficiency/services/water-api.service";
import { UserAPIService } from "@/shared/services/user-api.service";

import { withLatestFrom, combineLatest } from "rxjs";
import { ReportAPIService } from "@/shared/services/report-api.service";
import { MapViewService } from "@/shared/services/map-view.service";
import { FormBuilder, FormGroup } from "@angular/forms";
import { IWaterDeficiencyFilter } from "../../models/IWaterDeficiencyFilter";
import { IWaterDeficiency } from "../../models/IWaterDeficiency";
import { enumToSelectOptions } from "@/shared/helpers/enum-helper";
import { EDangerState, EDeficiencyType, EMapLayer } from "@/models/enums";
import { ISelectOption } from "@/shared/models/ISelectOption";
import { AuditService } from "@/shared/services/audit.service";
import { LoadingService } from "@/shared/services/loading.service";
import { sortIndicator, toggleSort } from "@/shared/helpers/table-sort.helper";
import { ITableSort } from "@/shared/models/ITableSort";

@Component({
  selector: "n-water-deficiencies",
  templateUrl: "./water-deficiency-table.component.html",
  styleUrls: ["../../../../shared/styles/table-list.component.css", "./water-deficiency-table.component.css"],
  standalone: false,
})
export class WaterDeficiencyTable {
  scrollCheckDisabled: boolean = false;
  filterForm!: FormGroup;
  isMonitoring: boolean = false;
  isLoading: boolean = true;
  isLoadingMore: boolean = false;
  sort: ITableSort | null = null;
  sortIndicator = sortIndicator;

  dangerStates: ISelectOption<EDangerState>[] = [];

  private listScrollCount = 0;
  private isFilterChanged: boolean = false;

  constructor(
    public waterAPIService: WaterAPIService,
    public auditService: AuditService,
    private userService: UserAPIService,
    private router: Router,
    private reportAPIService: ReportAPIService,
    private mapViewService: MapViewService,
    private fb: FormBuilder,
    private loadingService: LoadingService,
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

    this.userService.$user.subscribe((user) => {
      this.isMonitoring = user?.deficiencyMonitoringScheme?.isMonitoringWaterDeficiencies || false;
    });

    this.waterAPIService.deficiencies$.subscribe((deficiencies) => {
      if (this.listScrollCount === 0) {
        this.isLoading = false;
      }
    });

    this.waterAPIService.totalCount$.subscribe((totalCount) => {
      if (this.listScrollCount === 0 && totalCount === 0) {
        this.isLoading = false;
      }
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

  onSort(field: string) {
    this.sort = toggleSort(this.sort, field);
    this.listScrollCount = 0;
    this.isLoading = true;
    this.waterAPIService.loadWaterDeficiencies(0, this.filterForm.value, this.sort);
  }

  onScroll() {
    if (this.isLoadingMore) return;

    this.isLoadingMore = true;
    this.listScrollCount++;
    this.waterAPIService.loadWaterDeficiencies(this.listScrollCount, this.filterForm.value, this.sort);

    this.waterAPIService.deficiencies$
      .pipe(withLatestFrom(this.waterAPIService.totalCount$))
      .subscribe(([deficiencies, totalCount]) => {
        this.scrollCheckDisabled = totalCount <= deficiencies.length;
        this.isLoadingMore = false;
      });
  }

  goToSoil(isWaterSelected: boolean) {
    if (!isWaterSelected) {
      this.router.navigateByUrl("soil");
    }
  }

  changeMapFocus(deficiency: IWaterDeficiency) {
    this.mapViewService.changeFocus({ latitude: deficiency.latitude, longitude: deficiency.longitude }, 12, {
      layer: EMapLayer.WaterDeficiency,
      showSelectedMarker: false,
    });
  }

  applyFilter(): void {
    const filter: IWaterDeficiencyFilter = this.filterForm.value;

    if (this.isFilterChanged) {
      this.listScrollCount = 0;
      this.isLoading = true;
    }

    this.waterAPIService.loadWaterDeficiencies(this.listScrollCount, filter, this.sort);
  }

  toggleMonitoring() {
    this.auditService.toggleMonitoring(null, EDeficiencyType.Water).subscribe();
  }
}
