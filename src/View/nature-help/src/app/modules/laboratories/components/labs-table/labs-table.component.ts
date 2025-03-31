import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";

import { LabsAPIService } from "../../services/labs-api.service";
import { MapViewService } from "@/shared/services/map-view.service";
import { ILocation } from "@/models/ILocation";

@Component({
  selector: "nat-labs-table",
  templateUrl: "./labs-table.component.html",
  styleUrls: ["./labs-table.component.css"],
  standalone: false,
})
export class LabsTableComponent implements OnInit {
  public search: string = "";

  constructor(
    public labsAPI: LabsAPIService,
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
}
