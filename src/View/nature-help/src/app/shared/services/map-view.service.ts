import { Injectable } from '@angular/core';
import { DataSetWaterService } from '../../modules/water-deficiency/services/data-set-water.service';
import { DataSetSoilService } from '../../modules/soil-deficiency/services/data-set-soil.service';
import L, { LatLng } from 'leaflet';
import { IWaterDeficiency } from '@/modules/water-deficiency/models/IWaterDeficiency';
import { ISoilDeficiency } from '@/modules/soil-deficiency/models/ISoilDeficiency';
import { IDeficiency } from '@/models/IDeficiency';
@Injectable({
  providedIn: 'root'
})
export class MapViewService {
  private markedList: IDeficiency[] = [];
  // private soilsList: IDeficiency[] = [];

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

  constructor(private WaterDataService: DataSetWaterService, private SoilDataService: DataSetSoilService) {
  }

  public makeMarkers(defs?: IDeficiency[], color: string = 'blue'): void {
    if (defs){
      this.markedList = defs;
    }

    if (!this.markedList.length) {
      console.error('No deficiencies found to display.');
      return;
    }

    // this.watersList.map((def) => {
    //   const lon = def.location.longitude;
    //   const lat = def.location.latitude;

    //   this.makeMarker(def, lon, lat);
    // });

    this.markedList.forEach((def) => {
      const { longitude, latitude } = def.location;
      if (latitude && longitude) {
        this.makeMarker(def, longitude, latitude, color);
      } else {
        console.warn(`Deficiency ${def.title} does not have valid coordinates.`);
      }
    });
  }

  private makeMarker(def: IDeficiency, lon: number, lat: number, color: string): void {
    const circle = L.circleMarker([lat, lon], {
      radius: 8,
      color: color,
      opacity: 0.6,
      fillColor: color,
      fillOpacity: 0.2
    });

    circle.bindPopup(this.makePopup(def));

    circle.addTo(this.map);
  }

  public changeDeficiencyFocus(lat: number, lon: number, zoom: number){
    this.map.setView([lat, lon], zoom);
  }

  public fullScreenMap(){
    const mapObject = document.getElementById("map");

    mapObject!.style.height = "100%";
  }

  public makePopup(def: IDeficiency){
    return `
    <div>
      <b>${def.title}</b>
      <div>Type: ${def.type}</div>
      <div>Coordinates: ${def.location.latitude}, ${def.location.longitude}</div>
    </div>
  `;
  }
}
