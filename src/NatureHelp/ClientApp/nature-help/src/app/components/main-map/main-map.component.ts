import { Component, OnInit } from '@angular/core';
import { DataSetService } from '../../services/data-set.service';
import { MapViewService } from '../../services/map-view.service';

@Component({
  selector: 'n-main-map',
  templateUrl: './main-map.component.html',
  styleUrls: ['./main-map.component.css']
})
export class MainMapComponent implements OnInit{

  constructor(private stationsDataService: DataSetService,
    private mapViewService: MapViewService) { }

  ngOnInit (): void {
    this.mapViewService.initMap();

    this.stationsDataService.getAllStations()
      .subscribe(stations => this.mapViewService.makeStationMarkers(stations));
  }

  public goToFullScreen(){
    this.mapViewService.fullScreenMap();
  }

}
