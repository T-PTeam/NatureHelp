import { Router, NavigationEnd } from '@angular/router';
import { DataSetWaterService } from '@/modules/water-deficiency/services/data-set-water.service';
import { DataSetSoilService } from '@/modules/soil-deficiency/services/data-set-soil.service';
import { MapViewService } from '@/shared/services/map-view.service';
import { Component, OnInit } from '@angular/core';
import { MOCK_WATER_DEFICIENCIES } from '../../../modules/water-deficiency/components/water-deficiency-list/water-deficiency-list.component';
import { MOCK_SOIL_DEFICIENCIES } from '../../../modules/soil-deficiency/components/soil-deficiency-list/soil-deficiency-list.component';

@Component({
  selector: 'n-map',
  templateUrl: './main-map.component.html',
  styleUrls: ['./main-map.component.css'],
  standalone: false,
})
export class MapComponent implements OnInit{

  constructor(private waterDataService: DataSetWaterService, private soilDataService: DataSetSoilService,
    private mapViewService: MapViewService, private router: Router) { }

  ngOnInit (): void {
    this.mapViewService.initMap();

    // this.waterDataService.getAllWaterDeficiencies()
    //   .subscribe(defs => this.mapViewService.makeMarkers(defs));

    this.router.events.subscribe(() => {
      const routePath = this.router.url;

      if (routePath.includes('water')) {
        this.mapViewService.makeMarkers(MOCK_WATER_DEFICIENCIES, 'blue');
      } else if (routePath.includes('soil')) {
        this.mapViewService.makeMarkers(MOCK_SOIL_DEFICIENCIES, 'brown');
      }
    });
  }

  public goToFullScreen(){
    this.mapViewService.fullScreenMap();
  }

}
