import { DataSetWaterService } from '@/modules/water-deficiency/services/data-set-water.service';
import { DataSetSoilService } from '@/modules/soil-deficiency/services/data-set-soil.service';
import { MapViewService } from '@/shared/services/map-view.service';
import { Component, OnInit } from '@angular/core';
import { MOCK_WATER_DEFICIENCIES } from '../../../modules/water-deficiency/components/water-deficiency-list/water-deficiency-list.component';

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

    // this.waterDataService.getAllWaterDeficiencies()
    //   .subscribe(stations => this.mapViewService.makeStationMarkers(stations));

    this.mapViewService.makeStationMarkers(MOCK_WATER_DEFICIENCIES);

    // this.waterDataService.getAllWaterDeficiencies().subscribe((stations) => {
    //   if (stations) {
    //     this.mapViewService.makeStationMarkers(stations);
    //   } else {
    //     console.error('No water deficiencies data received.');
    //   }
    // });
  }

  public goToFullScreen(){
    this.mapViewService.fullScreenMap();
  }

}
