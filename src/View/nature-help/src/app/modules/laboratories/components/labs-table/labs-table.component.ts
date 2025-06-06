import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";

import { LabsAPIService } from "../../services/labs-api.service";
import { MapViewService } from "@/shared/services/map-view.service";
import { ILocation } from "@/models/ILocation";
import { withLatestFrom } from "rxjs";
import { ILaboratorFilter } from "../../models/ILaboratoryFilter";
import { FormGroup, FormBuilder } from "@angular/forms";

@Component({
  selector: "nat-labs-table",
  templateUrl: "./labs-table.component.html",
  styleUrls: ["./labs-table.component.css", "../../../../shared/styles/table-list.component.css"],
  standalone: false,
})
export class LabsTableComponent implements OnInit {
  public search: string = "";
  public scrollCheckDisabled: boolean = false;
  filterForm!: FormGroup;

  private listScrollCount = 0;
  private isFilterChanged: boolean = false;

  constructor(
    public labsAPIService: LabsAPIService,
    private router: Router,
    private mapViewService: MapViewService,
    private fb: FormBuilder,
  ) {
    this.filterForm = this.fb.group({
      location: this.fb.group({
        country: [""],
        region: [""],
        district: [""],
        city: [""],
      }),
    });

    this.filterForm.valueChanges.subscribe(() => {
      this.isFilterChanged = true;
    });
  }

  ngOnInit(): void {}

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

  changeMapFocus(location: ILocation) {
    this.mapViewService.changeFocus(location, 20);
  }

  onScroll() {
    this.listScrollCount++;
    this.labsAPIService.loadLabs(this.listScrollCount, this.filterForm.value);

    this.labsAPIService.labs$.pipe(withLatestFrom(this.labsAPIService.totalCount$)).subscribe(([labs, totalCount]) => {
      this.scrollCheckDisabled = totalCount <= labs.length;
    });
  }

  applyFilter(): void {
    const filter: ILaboratorFilter = this.filterForm.value;

    if (this.isFilterChanged) this.listScrollCount = 0;

    this.labsAPIService.loadLabs(this.listScrollCount, filter);
  }
}
