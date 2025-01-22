import { DataSetService } from '@/modules/water-deficiency/services/data-set.service';
import { MapViewService } from '@/shared/services/map-view.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'n-map',
  templateUrl: './main-map.component.html',
  styleUrls: ['./main-map.component.css'],
  standalone: false,
})
export class MapComponent implements OnInit{

  constructor(private stationsDataService: DataSetService,
    private mapViewService: MapViewService) { }

  ngOnInit (): void {
    this.mapViewService.initMap();

    this.stationsDataService.getAllWaterDeficiencies()
      .subscribe(stations => this.mapViewService.makeStationMarkers(stations));
  }

  public goToFullScreen(){
    this.mapViewService.fullScreenMap();
  }

}
