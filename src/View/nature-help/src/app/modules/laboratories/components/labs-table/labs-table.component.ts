import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";

import { LabsAPIService } from "../../services/labs-api.service";
import { MapViewService } from "@/shared/services/map-view.service";
import { ILocation } from "@/models/ILocation";
import { withLatestFrom } from "rxjs";

@Component({
  selector: "nat-labs-table",
  templateUrl: "./labs-table.component.html",
  styleUrls: ["./labs-table.component.css", "../../../../shared/styles/table-list.component.css"],
  standalone: false,
})
export class LabsTableComponent implements OnInit {
  public search: string = "";
  public scrollCheckDisabled: boolean = false;

  private listScrollCount = 0;

  constructor(
    public labsAPIService: LabsAPIService,
    private router: Router,
    private mapViewService: MapViewService,
  ) {}

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
    this.labsAPIService.loadLabs(this.listScrollCount);

    this.labsAPIService.labs$.pipe(withLatestFrom(this.labsAPIService.totalCount$)).subscribe(([labs, totalCount]) => {
      this.scrollCheckDisabled = totalCount <= labs.length;
    });
  }
}
