import { Injectable } from "@angular/core";
import * as L from "leaflet";
import { BehaviorSubject, combineLatest, map, Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";

import { ICoordinates } from "@/models/ICoordinates";
import { IWaterDeficiency } from "@/modules/water-deficiency/models/IWaterDeficiency";

import { SoilAPIService } from "../../modules/soil-deficiency/services/soil-api.service";
import { WaterAPIService } from "../../modules/water-deficiency/services/water-api.service";
import { LabsAPIService } from "@/modules/laboratories/services/labs-api.service";
import { ILaboratory } from "@/modules/laboratories/models/ILaboratory";
import { IDeficiency } from "@/models/IDeficiency";
import { EDangerState, EDeficiencyType, EMapLayer } from "@/models/enums";
import "leaflet.markercluster";

export interface IAddress {
  displayName: string;
  street?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: string;
}

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

  private selectedCoordinatesSubject = new BehaviorSubject<ICoordinates | null>(null);
  public selectedCoordinates$: Observable<ICoordinates | null> = this.selectedCoordinatesSubject.asObservable();

  private selectedAddressSubject = new BehaviorSubject<IAddress | null>(null);
  public selectedAddress$: Observable<IAddress | null> = this.selectedAddressSubject.asObservable();

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
    private http: HttpClient,
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

  private async lookupAddress(coordinates: ICoordinates): Promise<IAddress | null> {
    try {
      const response = await this.http
        .get<any>(
          `https://nominatim.openstreetmap.org/reverse?format=json&lat=${coordinates.latitude}&lon=${coordinates.longitude}&zoom=18&addressdetails=1`,
        )
        .toPromise();

      if (response) {
        const address = response.address;
        return {
          displayName: response.display_name,
          street: address.road || address.pedestrian || address.path,
          city: address.city || address.town || address.village,
          state: address.state,
          country: address.country,
          postalCode: address.postcode,
        };
      }
      return null;
    } catch (error) {
      console.error("Error looking up address:", error);
      return null;
    }
  }

  public enableCoordinateSelection() {
    // Change cursor style to crosshair
    const mapContainer = this.map.getContainer();
    mapContainer.style.cursor = "crosshair";
    mapContainer.classList.add("crosshair");

    this.map.on("click", async (e: L.LeafletMouseEvent) => {
      const coordinates: ICoordinates = {
        latitude: e.latlng.lat,
        longitude: e.latlng.lng,
      };
      this.selectedCoordinatesSubject.next(coordinates);

      // Look up address
      const address = await this.lookupAddress(coordinates);
      this.selectedAddressSubject.next(address);

      // Add a temporary marker to show the selected location
      const marker = L.marker([coordinates.latitude, coordinates.longitude], {
        icon: L.divIcon({
          className: "selected-location-marker",
          html: '<div class="selected-location-pin"></div>',
          iconSize: [20, 20],
        }),
      });

      // Remove any existing temporary markers
      this.map.eachLayer((layer: L.Layer) => {
        if (layer instanceof L.Marker && layer.getIcon()?.options.className === "selected-location-marker") {
          this.map.removeLayer(layer);
        }
      });

      marker.addTo(this.map);

      // Automatically disable coordinate selection after selection
      this.disableCoordinateSelection();
    });
  }

  public disableCoordinateSelection() {
    // Reset cursor style
    const mapContainer = this.map.getContainer();
    mapContainer.style.cursor = "";
    mapContainer.classList.remove("crosshair");

    this.map.off("click");
    this.selectedCoordinatesSubject.next(null);
    this.selectedAddressSubject.next(null);

    // Remove any temporary markers
    this.map.eachLayer((layer: L.Layer) => {
      if (layer instanceof L.Marker && layer.getIcon()?.options.className === "selected-location-marker") {
        this.map.removeLayer(layer);
      }
    });
  }
}
