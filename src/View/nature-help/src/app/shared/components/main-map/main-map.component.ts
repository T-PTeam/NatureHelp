import { Component, HostListener, OnInit } from "@angular/core";
import { MapViewService } from "@/shared/services/map-view.service";

@Component({
  selector: "nat-map",
  templateUrl: "./main-map.component.html",
  styleUrls: ["./main-map.component.css"],
  standalone: false,
})
export class MapComponent implements OnInit {
  constructor(private mapViewService: MapViewService) {}

  ngOnInit(): void {
    this.mapViewService.initMap();

    this.mapViewService.makeDeficiencyMarkers();
    this.mapViewService.makeLabMarkers();
  }
}
