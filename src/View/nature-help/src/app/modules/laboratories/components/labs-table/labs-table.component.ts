import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { FormGroup, FormBuilder } from "@angular/forms";
import { withLatestFrom } from "rxjs";

import { EMapLayer } from "@/models/enums";
import { sortIndicator, toggleSort } from "@/shared/helpers/table-sort.helper";
import { ITableSort } from "@/shared/models/ITableSort";
import { MapViewService } from "@/shared/services/map-view.service";
import { MobileMapService } from "@/shared/services/mobile-map.service";
import { UserAPIService } from "@/shared/services/user-api.service";
import { LabsAPIService } from "../../services/labs-api.service";
import { ILaboratorFilter } from "../../models/ILaboratoryFilter";
import { canEditLaboratory } from "@/shared/helpers/laboratory-access.helper";
import { ILaboratory } from "../../models/ILaboratory";

@Component({
  selector: "nat-labs-table",
  templateUrl: "./labs-table.component.html",
  styleUrls: ["../../../../shared/styles/table-list.component.css", "./labs-table.component.css"],
  standalone: false,
})
export class LabsTableComponent {
  public scrollCheckDisabled: boolean = false;
  public sort: ITableSort | null = null;
  public sortIndicator = sortIndicator;
  filterForm!: FormGroup;

  private listScrollCount = 0;
  private isFilterChanged: boolean = false;

  constructor(
    public labsAPIService: LabsAPIService,
    private router: Router,
    private mapViewService: MapViewService,
    private mobileMapService: MobileMapService,
    public userAPIService: UserAPIService,
    private fb: FormBuilder,
  ) {
    this.filterForm = this.fb.group({
      title: [""],
    });

    this.filterForm.valueChanges.subscribe(() => {
      this.isFilterChanged = true;
    });
  }

  public navigateToDetail(id?: string) {
    if (id) {
      this.router.navigate([`/labs/${id}`]);
    } else {
      this.router.navigate([`labs/add`]);
    }
  }

  switchTable() {
    this.router.navigateByUrl("researches");
  }

  changeMapFocus(lab: ILaboratory) {
    const focus = () =>
      this.mapViewService.changeFocus({ latitude: lab.latitude, longitude: lab.longitude }, 14, {
        layer: EMapLayer.Laboratories,
        popupHtml: `<strong>${lab.title}</strong>`,
      });

    if (this.mobileMapService.isMobile()) {
      this.mobileMapService.showMobileMap();
      setTimeout(focus, 150);
      return;
    }

    focus();
  }

  onSort(field: string) {
    this.sort = toggleSort(this.sort, field);
    this.listScrollCount = 0;
    this.labsAPIService.loadLabs(0, this.filterForm.value, this.sort);
  }

  onScroll() {
    this.listScrollCount++;
    this.labsAPIService.loadLabs(this.listScrollCount, this.filterForm.value, this.sort);

    this.labsAPIService.labs$.pipe(withLatestFrom(this.labsAPIService.totalCount$)).subscribe(([labs, totalCount]) => {
      this.scrollCheckDisabled = totalCount <= labs.length;
    });
  }

  applyFilter(): void {
    const filter: ILaboratorFilter = this.filterForm.value;

    if (this.isFilterChanged) this.listScrollCount = 0;

    this.labsAPIService.loadLabs(this.listScrollCount, filter, this.sort);
  }

  canEditLab(lab: ILaboratory): boolean {
    return canEditLaboratory(lab, sessionStorage.getItem("userId"), sessionStorage.getItem("role"));
  }
}
