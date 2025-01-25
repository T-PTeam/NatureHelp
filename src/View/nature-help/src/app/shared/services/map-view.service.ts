import { Injectable } from '@angular/core';
import { DataSetWaterService } from '../../modules/water-deficiency/services/data-set-water.service';
import { DataSetSoilService } from '../../modules/soil-deficiency/services/data-set-soil.service';
import L, { LatLng } from 'leaflet';
import { IWaterDeficiency } from '@/modules/water-deficiency/models/IWaterDeficiency';
import { ISoilDeficiency } from '@/modules/soil-deficiency/models/ISoilDeficiency';
@Injectable({
  providedIn: 'root'
})
export class MapViewService {
  private gasStationsList: IWaterDeficiency[] = [];

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

  public makeStationMarkers(stations?: IWaterDeficiency[]): void {
    if (stations){
      this.gasStationsList = stations;
    }

    if (!this.gasStationsList.length) {
      console.error('No stations found to display.');
      return;
    }

    // this.gasStationsList.map((station) => {
    //   const lon = station.location.longitude;
    //   const lat = station.location.latitude;

    //   this.makeMarker(station, lon, lat);
    // });

    this.gasStationsList.forEach((station) => {
      const { longitude, latitude } = station.location;
      if (latitude && longitude) {
        this.makeMarker(station, longitude, latitude);
      } else {
        console.warn(`Station ${station.title} does not have valid coordinates.`);
      }
    });
  }

  private makeMarker(station: IWaterDeficiency, lon: number, lat: number): void {
    const circle = L.circleMarker([lat, lon], {
      radius: 8,
      color: 'blue',
      fillColor: '#3f51b5',
      fillOpacity: 0.6,
    });

    circle.bindPopup(this.makeStationPopup(station));

    circle.addTo(this.map);
  }

  public changeStationFocus(lat: number, lon: number, zoom: number){
    this.map.setView([lat, lon], zoom);
  }

  public fullScreenMap(){
    const mapObject = document.getElementById("map");

    mapObject!.style.height = "100%";
  }

  public makeStationPopup(station: IWaterDeficiency){
    return `
    <div>
      <b>${station.title}</b>
      <div>Type: ${station.type}</div>
      <div>Coordinates: ${station.location.latitude}, ${station.location.longitude}</div>
    </div>
  `;
  }
}
