import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { SoilAPIService } from "@/modules/soil-deficiency/services/soil-api.service";
import { UserAPIService } from "@/shared/services/user-api.service";
import { withLatestFrom } from "rxjs";
import { ReportAPIService } from "@/shared/services/report-api.service";
import { MapViewService } from "@/shared/services/map-view.service";
import { ISoilDeficiencyFilter } from "../../models/ISoilDeficiencyFilter";
import { ISoilDeficiency } from "../../models/ISoilDeficiency";
import { enumToSelectOptions } from "@/shared/helpers/enum-helper";
import { EDangerState, EDeficiencyType, EMapLayer } from "@/models/enums";
import { ISelectOption } from "@/shared/models/ISelectOption";
import { FormGroup, FormBuilder } from "@angular/forms";
import { AuditService } from "@/shared/services/audit.service";
import { sortIndicator, toggleSort } from "@/shared/helpers/table-sort.helper";
import { ITableSort } from "@/shared/models/ITableSort";

@Component({
  selector: "n-soil-deficiencies",
  templateUrl: "./soil-deficiency-table.component.html",
  styleUrls: ["../../../../shared/styles/table-list.component.css", "./soil-deficiency-table.component.css"],
  standalone: false,
})
export class SoilDeficiencyTable {
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
    public soilAPIService: SoilAPIService,
    public auditService: AuditService,
    private userService: UserAPIService,
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

    this.userService.$user.subscribe((user) => {
      this.isMonitoring = user?.deficiencyMonitoringScheme?.isMonitoringSoilDeficiencies || false;
    });

    this.soilAPIService.deficiencies$.subscribe((deficiencies) => {
      if (this.listScrollCount === 0) {
        this.isLoading = false;
      }
    });

    this.soilAPIService.totalCount$.subscribe((totalCount) => {
      if (this.listScrollCount === 0 && totalCount === 0) {
        this.isLoading = false;
      }
    });
  }

  downloadExcel() {
    this.reportAPIService.downloadSoilDeficiencyExcelListFile();
  }

  navigateToDetail(id?: string) {
    if (id) {
      this.router.navigate([`/soil/${id}`]);
    } else {
      this.router.navigate([`soil/add`]);
    }
  }

  onSort(field: string) {
    this.sort = toggleSort(this.sort, field);
    this.listScrollCount = 0;
    this.isLoading = true;
    this.soilAPIService.loadSoilDeficiencies(0, this.filterForm.value, this.sort);
  }

  onScroll() {
    if (this.isLoadingMore) return;

    this.isLoadingMore = true;
    this.listScrollCount++;
    this.soilAPIService.loadSoilDeficiencies(this.listScrollCount, this.filterForm.value, this.sort);

    this.soilAPIService.deficiencies$
      .pipe(withLatestFrom(this.soilAPIService.totalCount$))
      .subscribe(([deficiencies, totalCount]) => {
        this.scrollCheckDisabled = totalCount <= deficiencies.length;
        this.isLoadingMore = false;
      });
  }

  goToWater(isWaterSelected: boolean) {
    if (isWaterSelected) {
      this.router.navigateByUrl("/water");
    }
  }

  changeMapFocus(deficiency: ISoilDeficiency) {
    this.mapViewService.changeFocus({ latitude: deficiency.latitude, longitude: deficiency.longitude }, 12, {
      layer: EMapLayer.SoilDeficiency,
      popupHtml: `<strong>${deficiency.title}</strong>`,
    });
  }

  applyFilter(): void {
    const filter: ISoilDeficiencyFilter = this.filterForm.value;

    if (this.isFilterChanged) {
      this.listScrollCount = 0;
      this.isLoading = true;
    }

    this.soilAPIService.loadSoilDeficiencies(this.listScrollCount, filter, this.sort);
  }

  toggleMonitoring(): void {
    this.auditService.toggleMonitoring(null, EDeficiencyType.Soil).subscribe();
  }
}
