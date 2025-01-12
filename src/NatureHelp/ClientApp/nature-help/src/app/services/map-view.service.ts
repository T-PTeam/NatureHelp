import { Injectable } from '@angular/core';
import { DataSetService } from './data-set.service';
import L from 'leaflet';
import { IGasStation } from '../models/IGasStation';

@Injectable({
  providedIn: 'root'
})
export class MapViewService {
  private gasStationsList: IGasStation[] = [];

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

  public makeStationMarkers(stations?: IGasStation[]): void {
    if (stations){
      this.gasStationsList = stations;
    }

    this.gasStationsList.map((station) => {
      const lon = station.longitude;
      const lat = station.latitude;

      this.makeMarker(station, lon, lat);
    });
  }

  private makeMarker(station: IGasStation, lon: number, lat: number){
    const circle = new L.CircleMarker([lat, lon]);
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

  public makeStationPopup(station: IGasStation){
    return `` +
      `<div><b>${ station.name }<b></div>` +
      `<div>Type: ${ station.productType.toString() }</div>` +
      `<div>Work from: ${ station.workTimeFrom }</div>` +
      `<div>Work to: ${ station.workTimeTo }</div>`
  }
}
