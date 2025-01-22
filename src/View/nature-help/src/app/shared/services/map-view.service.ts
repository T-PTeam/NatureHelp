import { Injectable } from '@angular/core';
import { DataSetService } from '../../modules/water-deficiency/services/data-set.service';
import L, { LatLng } from 'leaflet';
import { IWaterDeficiency } from '@/modules/water-deficiency/models/IWaterDeficiency';
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

  constructor(private stationsDataService: DataSetService) {
  }

  public makeStationMarkers(stations?: IWaterDeficiency[]): void {
    if (stations){
      this.gasStationsList = stations;
    }

    this.gasStationsList.map((station) => {
      const lon = station.location.longitude;
      const lat = station.location.latitude;

      this.makeMarker(station, lon, lat);
    });
  }

  private makeMarker(station: IWaterDeficiency, lon: number, lat: number){
    const circle = new L.CircleMarker([lat, lon], {radius: 10});
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
    return `` +
      `<div><b>${ station.title }<b></div>` +
      `<div>Type: ${ station.type }</div>`
  }
}
