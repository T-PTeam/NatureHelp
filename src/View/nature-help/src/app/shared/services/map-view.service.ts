import { Injectable } from "@angular/core";
import L, { Icon } from "leaflet";
import { combineLatest, map } from "rxjs";

import { ICoordinates } from "@/models/ICoordinates";
import { IWaterDeficiency } from "@/modules/water-deficiency/models/IWaterDeficiency";

import { SoilAPIService } from "../../modules/soil-deficiency/services/soil-api.service";
import { WaterAPIService } from "../../modules/water-deficiency/services/water-api.service";
import { LabsAPIService } from "@/modules/laboratories/services/labs-api.service";
import { ILocation } from "@/models/ILocation";
import { ILaboratory } from "@/modules/laboratories/models/ILaboratory";
import { IDeficiency } from "@/models/IDeficiency";
import { EDangerState, EDeficiencyType } from "@/models/enums";

@Injectable({
  providedIn: "root",
})
export class MapViewService {
  private markerList$ = combineLatest([this.waterDataService.deficiencies$, this.soilDataService.deficiencies$]).pipe(
    map(([waterList, soilList]) => [...waterList, ...soilList]),
  );

  private map: any;

  private labIcon = L.icon({
    iconUrl: "assets/icons/map/lab.png", // Path to your icon image
    iconSize: [32, 32], // Size of the icon
    iconAnchor: [16, 32], // Point of the icon that corresponds to the marker's location
    popupAnchor: [0, -32], // Point from which the popup should open
  });

  public initMap(): void {
    this.map = L.map("map", {
      center: [48.65, 22.26],
      zoom: 13,
    });

    const tiles = L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
      maxZoom: 18,
      minZoom: 3,
      attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>',
    });

    tiles.addTo(this.map);
  }

  constructor(
    private waterDataService: WaterAPIService,
    private soilDataService: SoilAPIService,
    private labsAPIService: LabsAPIService,
  ) {}

  public changeFocus(coordinates: ICoordinates, zoom: number) {
    this.map.setView([coordinates.latitude, coordinates.longitude], zoom);
  }

  public fullScreenMap() {
    const mapObject = document.getElementById("map");

    mapObject!.style.height = "100%";
  }

  public makeDeficiencyMarkers(): void {
    this.markerList$
      .pipe(
        map((list) => {
          if (list && list.length) {
            list.forEach((d) => {
              if (this.isWaterDeficiency(d)) {
                this.makeCircleMarker(d.location, "blue", this.getDeficiencyPopup(d));
              } else {
                this.makeCircleMarker(d.location, "brown", this.getDeficiencyPopup(d));
              }
            });
          }
        }),
      )
      .subscribe();
  }

  public makeLabMarkers(): void {
    this.labsAPIService.labs$
      .pipe(
        map((list) => {
          list.map((lab) => this.makeIconMarker(lab.location, this.labIcon, this.getLabPopup(lab)));
        }),
      )
      .subscribe();
  }

  private makeCircleMarker(location: ILocation, color: string, popupTags: string | null = null): void {
    const circle = L.circleMarker([location.latitude ?? 50.4501, location.longitude ?? 30.5234], {
      radius: location.radiusAffected ?? 10,
      color: color,
      opacity: 0.6,
      fillColor: color,
      fillOpacity: 0.2,
    });

    if (popupTags) circle.bindPopup(popupTags);

    circle.addTo(this.map);
  }

  private makeIconMarker(location: ILocation, icon: Icon, popupTags: string | null = null): void {
    const circle = L.marker([location.latitude ?? 50.4501, location.longitude ?? 30.5234], {
      opacity: 0.6,
      icon: icon,
    });

    if (popupTags) circle.bindPopup(popupTags);

    circle.addTo(this.map);
  }

  getDeficiencyPopup(item: IDeficiency) {
    return `
      <div>
        <h3>${item.title}</h3>
        <p><strong>Description:</strong> ${item.description}</p>
        <p><strong>Type:</strong> ${EDeficiencyType[item.type]}</p>
        <p><strong>Danger Level:</strong> ${EDangerState[item.eDangerState]}</p>
        <p><strong>Location:</strong> ${item.location.latitude}, ${item.location.longitude}</p>
        <p><strong>Creator:</strong> ${item.creator.firstName} ${item.creator.lastName}</p>
        ${item.responsibleUser ? `<p><strong>Responsible User:</strong> ${item.responsibleUser.firstName} ${item.responsibleUser.lastName}</p>` : ""}
      </div>
    `;
  }

  getLabPopup(item: ILaboratory) {
    return `
      <div>
        <h3>${item.title}</h3>
        <p><strong>Location:</strong> ${item.location.latitude}, ${item.location.longitude}</p>
        <p><strong>Researchers:</strong> ${item.researchersCount}</p>
        <ul>
          ${item.researchers.map((r) => `<li>${r.firstName} ${r.lastName}</li>`).join("")}
        </ul>
      </div>
    `;
  }

  private isWaterDeficiency(obj: any): obj is IWaterDeficiency {
    console.log("isWaterDeficiency: ", obj);

    return (obj as IWaterDeficiency).microbialLoad !== undefined;
  }
}
