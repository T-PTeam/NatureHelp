import { Injectable } from '@angular/core';
import { WaterAPIService } from '../../modules/water-deficiency/services/water-api.service';
import { SoilAPIService } from '../../modules/soil-deficiency/services/soil-api.service';
import L, { LatLng, popup } from 'leaflet';
import { IWaterDeficiency } from '@/modules/water-deficiency/models/IWaterDeficiency';
import { ISoilDeficiency } from '@/modules/soil-deficiency/models/ISoilDeficiency';
import { IDeficiency } from '@/models/IDeficiency';
import { ICoordinates } from '@/models/ICoordinates';
import { combineLatest, map } from 'rxjs';
import { ILaboratory } from '@/modules/laboratories/models/ILaboratory';
@Injectable({
  providedIn: 'root'
})
export class MapViewService {
  private markerList$ = combineLatest([
    this.waterDataService.deficiencies$,
    this.soilDataService.deficiencies$
  ]).pipe(
    map(([waterList, soilList]) => [...waterList, ...soilList])
  );

  private map: any;

  public initMap(): void {
    this.map = L.map('map', {
      center: [ 48.65, 	22.26 ],
      zoom: 13
    });

    const tiles = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 18,
      minZoom: 3,
      attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    });

    tiles.addTo(this.map);
  }

  constructor(private waterDataService: WaterAPIService, private soilDataService: SoilAPIService) {
  }

  private isWaterDeficiency(obj: any): obj is IWaterDeficiency {
    return (obj as IWaterDeficiency).microbialLoad !== undefined;
  }

  private isSoilDeficiency(obj: any): obj is ISoilDeficiency {
    return (obj as ISoilDeficiency).organicMatter !== undefined;
  }

  private isLaboratory(obj: any): obj is ILaboratory {
    return (obj as ILaboratory).researches !== undefined;
  }

  public makeMarkers(): void {
    this.markerList$.pipe(
      map(list => {
        if (Array.isArray(list)) {
          if (this.isWaterDeficiency(list[0])) {
            list.map(def => this.makeMarker(def, def.location, 'blue'));
          } else if (this.isSoilDeficiency(list[0])) {
            list.map(def => this.makeMarker(def, def.location, 'brown'));
          } else if (this.isLaboratory(list[0])) {
            list.map(def => this.makeMarker(def, def.location, 'green'));
          }
        }
      })
    ).subscribe();
  }


  private makeMarker(
    def: IDeficiency, 
    coordinates: ICoordinates,
    color: string,
    popupTags: string | null = null): void 
  {
    const circle = L.circleMarker([coordinates.latitude, coordinates.longitude], {
      radius: 20,
      color: color,
      opacity: 0.6,
      fillColor: color,
      fillOpacity: 0.2
    });

    if (popupTags) circle.bindPopup(popupTags);

    circle.addTo(this.map);
  }

  public changeFocus(coordinates: ICoordinates, zoom: number){
    this.map.setView([coordinates.latitude, coordinates.longitude], zoom);
  }

  public fullScreenMap(){
    const mapObject = document.getElementById("map");

    mapObject!.style.height = "100%";
  }
}
