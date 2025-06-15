import { Injectable } from "@angular/core";
import * as L from "leaflet";
import { combineLatest, map } from "rxjs";

import { ICoordinates } from "@/models/ICoordinates";
import { IWaterDeficiency } from "@/modules/water-deficiency/models/IWaterDeficiency";

import { SoilAPIService } from "../../modules/soil-deficiency/services/soil-api.service";
import { WaterAPIService } from "../../modules/water-deficiency/services/water-api.service";
import { LabsAPIService } from "@/modules/laboratories/services/labs-api.service";
import { ILaboratory } from "@/modules/laboratories/models/ILaboratory";
import { IDeficiency } from "@/models/IDeficiency";
import { EDangerState, EDeficiencyType, EMapLayer } from "@/models/enums";
import "leaflet.markercluster";

@Injectable({
  providedIn: "root",
})
export class MapViewService {
  osm = L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
    attribution: "OpenStreetMap",
  });

  satellite = L.tileLayer("https://{s}.google.com/vt/lyrs=s&x={x}&y={y}&z={z}", {
    subdomains: ["mt0", "mt1", "mt2", "mt3"],
    attribution: "Google Satellite",
  });

  laboratoriesLayer = L.markerClusterGroup({
    disableClusteringAtZoom: 17,
    iconCreateFunction: function (cluster: L.MarkerCluster) {
      const count = cluster.getChildCount();

      return L.divIcon({
        html: `<div class="cluster lab-cluster">${count}</div>`,
        className: "cluster-wrapper",
        iconSize: [40, 40],
      });
    },
  });
  waterDeficienciesLayer = L.markerClusterGroup({
    disableClusteringAtZoom: 17,
    iconCreateFunction: function (cluster: L.MarkerCluster) {
      const count = cluster.getChildCount();

      return L.divIcon({
        html: `<div class="cluster water-def-cluster">${count}</div>`,
        className: "cluster-wrapper",
        iconSize: [40, 40],
      });
    },
  });
  soilDeficienciesLayer = L.markerClusterGroup({
    disableClusteringAtZoom: 17,
    iconCreateFunction: function (cluster: L.MarkerCluster) {
      const count = cluster.getChildCount();

      return L.divIcon({
        html: `<div class="cluster soil-def-cluster">${count}</div>`,
        className: "cluster-wrapper",
        iconSize: [40, 40],
      });
    },
  });

  baseMaps = {
    Scheme: this.osm,
    Satellite: this.satellite,
  };

  overlayMaps = {
    Laboratories: this.laboratoriesLayer,
    "Water Deficiencies": this.waterDeficienciesLayer,
    "Soil Deficiencies": this.soilDeficienciesLayer,
  };

  private markerList$ = combineLatest([this.waterDataService.deficiencies$, this.soilDataService.deficiencies$]).pipe(
    map(([waterList, soilList]) => [...waterList, ...soilList]),
  );

  private map: any;

  private labIcon = L.icon({
    iconUrl: "assets/icons/map/lab.png",
    iconSize: [32, 32],
    iconAnchor: [16, 32],
    popupAnchor: [0, -32],
  });

  public initMap(): void {
    this.map = L.map("map", {
      center: [48.65, 22.26],
      zoom: 13,
      maxZoom: 18,
    });

    L.control.layers(this.baseMaps, this.overlayMaps).addTo(this.map);

    this.addAllLayersToMap(); // TODO: it makes double markers

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
            this.clearDeficienciesLayersFromMap();

            list.forEach((d) => {
              if (this.isWaterDeficiency(d)) {
                this.makeCircleMarker(
                  EMapLayer.WaterDeficiency,
                  { longitude: d.longitude, latitude: d.latitude },
                  "#4285f4",
                  this.getDeficiencyPopup(d),
                );
              } else {
                this.makeCircleMarker(
                  EMapLayer.SoilDeficiency,
                  { longitude: d.longitude, latitude: d.latitude },
                  "brown",
                  this.getDeficiencyPopup(d),
                );
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
          this.laboratoriesLayer.clearLayers();

          list.map((lab) =>
            this.makeIconMarker(
              EMapLayer.Laboratories,
              { longitude: lab.longitude, latitude: lab.latitude },
              this.labIcon,
              this.getLabPopup(lab),
            ),
          );
        }),
      )
      .subscribe();
  }

  private makeCircleMarker(
    mapLayer: EMapLayer,
    coordinates: ICoordinates,
    color: string,
    popupTags: string | null = null,
  ): void {
    const circle = L.circleMarker([coordinates.latitude ?? 50.4501, coordinates.longitude ?? 30.5234], {
      radius: 10,
      color: color,
      opacity: 0.6,
      fillColor: color,
      fillOpacity: 0.2,
    });

    if (popupTags) circle.bindPopup(popupTags);

    switch (mapLayer) {
      case EMapLayer.WaterDeficiency:
        this.waterDeficienciesLayer.addLayer(circle);
        break;
      case EMapLayer.SoilDeficiency:
        this.soilDeficienciesLayer.addLayer(circle);
        break;
      case EMapLayer.Laboratories:
        this.laboratoriesLayer.addLayer(circle);
        break;
      default:
        circle.addTo(this.map);
        break;
    }
  }

  private makeIconMarker(
    mapLayer: EMapLayer,
    coordinates: ICoordinates,
    icon: L.Icon,
    popupTags: string | null = null,
  ): void {
    const circle = L.marker([coordinates.latitude ?? 50.4501, coordinates.longitude ?? 30.5234], {
      opacity: 0.6,
      icon: icon,
    });

    if (popupTags) circle.bindPopup(popupTags);

    switch (mapLayer) {
      case EMapLayer.WaterDeficiency:
        this.waterDeficienciesLayer.addLayer(circle);
        break;
      case EMapLayer.SoilDeficiency:
        this.soilDeficienciesLayer.addLayer(circle);
        break;
      case EMapLayer.Laboratories:
        this.laboratoriesLayer.addLayer(circle);
        break;
      default:
        circle.addTo(this.map);
        break;
    }
  }

  getDeficiencyPopup(item: IDeficiency) {
    return `
      <div>
        <h3>${item.title}</h3>
        <p><strong>Description:</strong> ${item.description}</p>
        <p><strong>Type:</strong> ${EDeficiencyType[item.type]}</p>
        <p><strong>Danger Level:</strong> ${EDangerState[item.eDangerState]}</p>
        <p><strong>Location:</strong> ${item.latitude}, ${item.longitude}</p>
        <p><strong>Creator:</strong> ${item.creator.firstName} ${item.creator.lastName}</p>
        ${item.responsibleUser ? `<p><strong>Responsible User:</strong> ${item.responsibleUser.firstName} ${item.responsibleUser.lastName}</p>` : ""}
      </div>
    `;
  }

  getLabPopup(item: ILaboratory) {
    return `
      <div>
        <h3>${item.title}</h3>
        <p><strong>Location:</strong> ${item.latitude}, ${item.longitude}</p>
        <p><strong>Researchers:</strong> ${item.researchersCount}</p>
        <ul>
          ${item.researchers.map((r) => `<li>${r.firstName} ${r.lastName}</li>`).join("")}
        </ul>
      </div>
    `;
  }

  private isWaterDeficiency(obj: any): obj is IWaterDeficiency {
    return (obj as IWaterDeficiency).microbialLoad !== undefined;
  }

  private addAllLayersToMap() {
    this.waterDeficienciesLayer.addTo(this.map);
    this.soilDeficienciesLayer.addTo(this.map);
    this.laboratoriesLayer.addTo(this.map);
  }

  private clearDeficienciesLayersFromMap() {
    this.waterDeficienciesLayer.clearLayers();
    this.soilDeficienciesLayer.clearLayers();
  }
}
