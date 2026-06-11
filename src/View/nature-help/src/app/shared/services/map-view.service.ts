import { Injectable } from "@angular/core";
import * as L from "leaflet";
import { BehaviorSubject, combineLatest, map, Observable } from "rxjs";

import { ICoordinates } from "@/models/ICoordinates";
import { IDeficiencyMapDto } from "@/models/IDeficiencyMapDto";

import { SoilAPIService } from "../../modules/soil-deficiency/services/soil-api.service";
import { WaterAPIService } from "../../modules/water-deficiency/services/water-api.service";
import { LabsAPIService } from "@/modules/laboratories/services/labs-api.service";
import { ILaboratoryMapDto } from "@/modules/laboratories/models/ILaboratoryMapDto";
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

interface INominatimAddress {
  house_number?: string;
  road?: string;
  pedestrian?: string;
  path?: string;
  city?: string;
  town?: string;
  village?: string;
  hamlet?: string;
  state?: string;
  postcode?: string;
  country?: string;
}

interface INominatimResponse {
  display_name?: string;
  address?: INominatimAddress;
}

interface IBigDataCloudResponse {
  locality?: string;
  city?: string;
  principalSubdivision?: string;
  postcode?: string;
  countryName?: string;
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

  private markerList$ = combineLatest([
    this.waterDataService.mapDeficiencies$,
    this.soilDataService.mapDeficiencies$,
  ]).pipe(map(([waterList, soilList]) => [...waterList, ...soilList]));

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

  constructor(
    private waterDataService: WaterAPIService,
    private soilDataService: SoilAPIService,
    private labsAPIService: LabsAPIService,
  ) {}

  public initMap(): void {
    if (this.map) {
      this.map.remove();
      this.map = undefined;
    }

    this.map = L.map("map", {
      center: [48.65, 22.26],
      zoom: 13,
      maxZoom: 18,
    });

    L.control.layers(this.baseMaps, this.overlayMaps).addTo(this.map);

    this.addAllLayersToMap();

    const tiles = L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
      maxZoom: 18,
      minZoom: 3,
      attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>',
    });

    tiles.addTo(this.map);
  }

  public changeFocus(coordinates: ICoordinates, zoom: number, options?: { layer?: EMapLayer; popupHtml?: string }) {
    if (!this.map) {
      return;
    }

    const latitude = Number(coordinates.latitude);
    const longitude = Number(coordinates.longitude);

    if (Number.isNaN(latitude) || Number.isNaN(longitude)) {
      return;
    }

    const normalized: ICoordinates = { latitude, longitude };

    if (options?.layer) {
      this.ensureLayerVisible(options.layer);
    }

    this.map.invalidateSize();
    this.map.setView([latitude, longitude], zoom);
    this.showSelectedLocationMarker(normalized, options?.popupHtml);

    if (options?.layer) {
      this.revealLayerMarker(options.layer, normalized);
    }
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
              const radius = d.radiusAffected > 0 ? d.radiusAffected : 10;
              if (this.isWaterDeficiency(d)) {
                this.makeCircleMarker(
                  EMapLayer.WaterDeficiency,
                  { longitude: d.longitude, latitude: d.latitude },
                  "#4285f4",
                  this.getDeficiencyPopup(d),
                  radius,
                );
              } else {
                this.makeCircleMarker(
                  EMapLayer.SoilDeficiency,
                  { longitude: d.longitude, latitude: d.latitude },
                  "brown",
                  this.getDeficiencyPopup(d),
                  radius,
                );
              }
            });
          }
        }),
      )
      .subscribe();
  }

  public makeLabMarkers(): void {
    this.labsAPIService.mapLabs$
      .pipe(
        map((list) => {
          this.laboratoriesLayer.clearLayers();

          if (list && list.length > 0) {
            list.forEach((lab) => {
              this.makeIconMarker(
                EMapLayer.Laboratories,
                { longitude: lab.longitude, latitude: lab.latitude },
                this.labIcon,
                this.getLabPopup(lab),
              );
            });
          }
        }),
      )
      .subscribe();
  }

  private makeCircleMarker(
    mapLayer: EMapLayer,
    coordinates: ICoordinates,
    color: string,
    popupTags: string | null = null,
    radius: number = 10,
  ): void {
    const circle = L.circleMarker([coordinates.latitude ?? 50.4501, coordinates.longitude ?? 30.5234], {
      radius: radius,
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

  getDeficiencyPopup(item: IDeficiencyMapDto) {
    return `
      <div>
        <h3>${item.title}</h3>
        <p><strong>Description:</strong> ${item.description}</p>
        <p><strong>Type:</strong> ${EDeficiencyType[item.type]}</p>
        <p><strong>Danger Level:</strong> ${EDangerState[item.eDangerState]}</p>
        <p><strong>Location:</strong> ${item.latitude}, ${item.longitude}</p>
        <p><strong>Creator:</strong> ${item.creatorFullName}</p>
        ${item.responsibleUserFullName ? `<p><strong>Responsible User:</strong> ${item.responsibleUserFullName}</p>` : ""}
      </div>
    `;
  }

  getLabPopup(item: ILaboratoryMapDto) {
    return `
      <div>
        <h3>${item.title}</h3>
        <p><strong>Location:</strong> ${item.latitude}, ${item.longitude}</p>
        <p><strong>Researchers:</strong> ${item.researchersCount}</p>
        <ul>
          ${item.researchers.map((r) => `<li>${r.fullName}</li>`).join("")}
        </ul>
      </div>
    `;
  }

  private isWaterDeficiency(obj: any): boolean {
    return obj.type === EDeficiencyType.Water;
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

  private async fetchJson<T>(url: string, headers?: Record<string, string>): Promise<T | null> {
    try {
      const response = await fetch(url, { headers });
      if (!response.ok) {
        return null;
      }

      return (await response.json()) as T;
    } catch (error) {
      console.error("Geocoding request failed:", error);
      return null;
    }
  }

  private buildDisplayNameFromNominatimParts(address: INominatimAddress): string {
    const street = [address.house_number, address.road || address.pedestrian || address.path].filter(Boolean).join(" ");
    const city = address.city || address.town || address.village || address.hamlet;

    return [street, city, address.state, address.postcode, address.country].filter(Boolean).join(", ");
  }

  private buildDisplayNameFromBigDataCloud(response: IBigDataCloudResponse): string {
    const parts = [
      response.locality,
      response.city !== response.locality ? response.city : undefined,
      response.principalSubdivision,
      response.postcode,
      response.countryName,
    ].filter((part, index, array) => part && array.indexOf(part) === index);

    return parts.join(", ");
  }

  private async lookupAddress(coordinates: ICoordinates): Promise<IAddress | null> {
    const bigDataCloudAddress = await this.lookupAddressFromBigDataCloud(coordinates);
    if (bigDataCloudAddress) {
      return bigDataCloudAddress;
    }

    return this.lookupAddressFromNominatim(coordinates);
  }

  private async lookupAddressFromNominatim(coordinates: ICoordinates): Promise<IAddress | null> {
    const response = await this.fetchJson<INominatimResponse>(
      `https://nominatim.openstreetmap.org/reverse?format=json&lat=${coordinates.latitude}&lon=${coordinates.longitude}&zoom=18&addressdetails=1`,
      { "Accept-Language": "en" },
    );

    if (!response) {
      return null;
    }

    const address = response.address ?? {};
    const displayName = response.display_name || this.buildDisplayNameFromNominatimParts(address);

    if (!displayName) {
      return null;
    }

    return {
      displayName,
      street: address.road || address.pedestrian || address.path,
      city: address.city || address.town || address.village,
      state: address.state,
      country: address.country,
      postalCode: address.postcode,
    };
  }

  private async lookupAddressFromBigDataCloud(coordinates: ICoordinates): Promise<IAddress | null> {
    const response = await this.fetchJson<IBigDataCloudResponse>(
      `https://api.bigdatacloud.net/data/reverse-geocode-client?latitude=${coordinates.latitude}&longitude=${coordinates.longitude}&localityLanguage=en`,
    );

    if (!response) {
      return null;
    }

    const displayName = this.buildDisplayNameFromBigDataCloud(response);

    if (!displayName) {
      return null;
    }

    return {
      displayName,
      city: response.city || response.locality,
      state: response.principalSubdivision,
      country: response.countryName,
      postalCode: response.postcode,
    };
  }

  private removeSelectedLocationMarker(): void {
    this.map.eachLayer((layer: L.Layer) => {
      if (layer instanceof L.Marker && layer.getIcon()?.options.className === "selected-location-marker") {
        this.map.removeLayer(layer);
      }
    });
  }

  private stopCoordinateSelectionMode(): void {
    const mapContainer = this.map.getContainer();
    mapContainer.style.cursor = "";
    mapContainer.classList.remove("crosshair");
    this.map.off("click");
  }

  public highlightSelectedCoordinates(coordinates: ICoordinates, popupHtml?: string): void {
    if (!this.map) {
      return;
    }

    const latitude = Number(coordinates.latitude);
    const longitude = Number(coordinates.longitude);

    if (Number.isNaN(latitude) || Number.isNaN(longitude)) {
      return;
    }

    this.showSelectedLocationMarker({ latitude, longitude }, popupHtml);
  }

  private showSelectedLocationMarker(coordinates: ICoordinates, popupHtml?: string): void {
    this.removeSelectedLocationMarker();

    const marker = L.marker([coordinates.latitude, coordinates.longitude], {
      icon: L.divIcon({
        className: "selected-location-marker",
        html: '<div class="selected-location-pin"></div>',
        iconSize: [20, 20],
        iconAnchor: [10, 10],
      }),
      zIndexOffset: 1000,
    }).addTo(this.map);

    if (popupHtml) {
      marker.bindPopup(popupHtml).openPopup();
    }
  }

  private ensureLayerVisible(layer: EMapLayer): void {
    const clusterGroup = this.getClusterGroup(layer);
    if (clusterGroup && !this.map.hasLayer(clusterGroup)) {
      clusterGroup.addTo(this.map);
    }
  }

  private getClusterGroup(layer: EMapLayer): L.MarkerClusterGroup | null {
    switch (layer) {
      case EMapLayer.Laboratories:
        return this.laboratoriesLayer;
      case EMapLayer.WaterDeficiency:
        return this.waterDeficienciesLayer;
      case EMapLayer.SoilDeficiency:
        return this.soilDeficienciesLayer;
      default:
        return null;
    }
  }

  private revealLayerMarker(layer: EMapLayer, coordinates: ICoordinates): void {
    const clusterGroup = this.getClusterGroup(layer);
    if (!clusterGroup) {
      return;
    }

    let matchedLayer: (L.Marker | L.CircleMarker) | null = null;

    clusterGroup.eachLayer((layerItem: L.Layer) => {
      if (!(layerItem instanceof L.Marker) && !(layerItem instanceof L.CircleMarker)) {
        return;
      }

      const { lat, lng } = layerItem.getLatLng();
      if (this.areCoordinatesClose(lat, lng, coordinates.latitude, coordinates.longitude)) {
        matchedLayer = layerItem;
      }
    });

    if (matchedLayer) {
      clusterGroup.zoomToShowLayer(matchedLayer, () => {
        matchedLayer?.openPopup();
      });
    }
  }

  private areCoordinatesClose(lat: number, lng: number, targetLat: number, targetLng: number): boolean {
    return Math.abs(lat - targetLat) < 0.0001 && Math.abs(lng - targetLng) < 0.0001;
  }

  public enableCoordinateSelection() {
    this.stopCoordinateSelectionMode();
    this.selectedAddressSubject.next(null);

    const mapContainer = this.map.getContainer();
    mapContainer.style.cursor = "crosshair";
    mapContainer.classList.add("crosshair");

    this.map.on("click", async (e: L.LeafletMouseEvent) => {
      const coordinates: ICoordinates = {
        latitude: e.latlng.lat,
        longitude: e.latlng.lng,
      };

      const address = await this.lookupAddress(coordinates);

      this.selectedCoordinatesSubject.next(coordinates);
      this.selectedAddressSubject.next(address);
      this.showSelectedLocationMarker(coordinates, address?.displayName);
      this.stopCoordinateSelectionMode();
    });
  }

  public disableCoordinateSelection() {
    this.stopCoordinateSelectionMode();
  }
}
