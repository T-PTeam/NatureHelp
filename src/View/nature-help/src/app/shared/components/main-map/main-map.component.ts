import { DataSetWaterService } from '@/modules/water-deficiency/services/data-set-water.service';
import { DataSetSoilService } from '@/modules/soil-deficiency/services/data-set-soil.service';
import { MapViewService } from '@/shared/services/map-view.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'n-map',
  templateUrl: './main-map.component.html',
  styleUrls: ['./main-map.component.css'],
  standalone: false,
})
export class MapComponent implements OnInit{

  constructor(private waterDataService: DataSetWaterService, private soilDataService: DataSetSoilService,
    private mapViewService: MapViewService) { }

  ngOnInit (): void {
    this.mapViewService.initMap();

    this.waterDataService.getAllWaterDeficiencies()
      .subscribe(stations => this.mapViewService.makeStationMarkers(stations));
  }

  public goToFullScreen(){
    this.mapViewService.fullScreenMap();
  }

}
