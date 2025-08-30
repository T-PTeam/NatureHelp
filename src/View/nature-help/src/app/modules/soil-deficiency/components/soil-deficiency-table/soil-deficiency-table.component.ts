import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { SoilAPIService } from "@/modules/soil-deficiency/services/soil-api.service";
import { UserAPIService } from "@/shared/services/user-api.service";
import { withLatestFrom } from "rxjs";
import { ReportAPIService } from "@/shared/services/report-api.service";
import { MapViewService } from "@/shared/services/map-view.service";
import { ISoilDeficiencyFilter } from "../../models/ISoilDeficiencyFilter";
import { enumToSelectOptions } from "@/shared/helpers/enum-helper";
import { EDangerState, EDeficiencyType } from "@/models/enums";
import { ISelectOption } from "@/shared/models/ISelectOption";
import { FormGroup, FormBuilder } from "@angular/forms";
import { AuditService } from "@/shared/services/audit.service";

@Component({
  selector: "n-soil-deficiencies",
  templateUrl: "./soil-deficiency-table.component.html",
  styleUrls: ["../../../../shared/styles/table-list.component.css", "./soil-deficiency-table.component.css"],
  standalone: false,
})
export class SoilDeficiencyTable {
  search: string = "";
  scrollCheckDisabled: boolean = false;
  filterForm!: FormGroup;
  isMonitoring: boolean = false;
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

  onScroll() {
    this.listScrollCount++;
    this.soilAPIService.loadSoilDeficiencies(this.listScrollCount, this.filterForm.value);

    this.soilAPIService.deficiencies$
      .pipe(withLatestFrom(this.soilAPIService.totalCount$))
      .subscribe(([deficiencies, totalCount]) => {
        this.scrollCheckDisabled = totalCount <= deficiencies.length;
      });
  }

  goToWater() {
    this.router.navigateByUrl("/water");
  }

  changeMapFocus(latitude: number, longitude: number) {
    this.mapViewService.changeFocus({ latitude, longitude }, 12);
  }

  applyFilter(): void {
    const filter: ISoilDeficiencyFilter = this.filterForm.value;

    if (this.isFilterChanged) this.listScrollCount = 0;

    this.soilAPIService.loadSoilDeficiencies(this.listScrollCount, filter);
  }

  toggleMonitoring(): void {
    this.auditService.toggleMonitoring(null, EDeficiencyType.Soil).subscribe();
  }
}
