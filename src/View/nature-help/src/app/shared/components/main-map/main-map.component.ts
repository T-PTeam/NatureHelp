import { Router, NavigationEnd } from '@angular/router';
import { WaterAPIService } from '@/modules/water-deficiency/services/waterAPI.service';
import { SoilAPIService } from '@/modules/soil-deficiency/services/soilAPI.service';
import { MapViewService } from '@/shared/services/map-view.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'n-map',
  templateUrl: './main-map.component.html',
  styleUrls: ['./main-map.component.css'],
  standalone: false,
})
export class MapComponent implements OnInit{

  constructor(
    private waterDataService: WaterAPIService,
    private soilDataService: SoilAPIService,
    private mapViewService: MapViewService,
    private router: Router
  ) {}

  ngOnInit (): void {
    this.mapViewService.initMap();

    this.router.events.subscribe(() => {
      const routePath = this.router.url;

      if (routePath.includes('water')) {
        this.loadWaterDeficiencies();
      } else if (routePath.includes('soil')) {
        this.loadSoilDeficiencies();
      }
    });
  }

  private loadWaterDeficiencies(): void {
    this.waterDataService.getAllWaterDeficiencies().subscribe(
      (data) => {
        this.mapViewService.makeMarkers(data, 'blue');
      },
      (error) => {
        console.error('Error: ', error);
      }
    );
  }

  private loadSoilDeficiencies(): void {
    this.soilDataService.getAllSoilDeficiencies().subscribe(
      (data) => {
        this.mapViewService.makeMarkers(data, 'brown');
      },
      (error) => {
        console.error('Error:', error);
      }
    );
  }

  public goToFullScreen(){
    this.mapViewService.fullScreenMap();
  }

}
