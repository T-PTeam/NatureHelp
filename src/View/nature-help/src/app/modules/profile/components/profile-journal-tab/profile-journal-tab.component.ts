import { Component, OnDestroy, OnInit, AfterViewInit, ElementRef, ViewChild, NgZone } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { TranslateModule } from "@ngx-translate/core";
import { MatCardModule } from "@angular/material/card";
import { MatIconModule } from "@angular/material/icon";
import { MatTableModule } from "@angular/material/table";
import { MatButtonModule } from "@angular/material/button";
import { MatButtonToggleModule } from "@angular/material/button-toggle";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";
import * as L from "leaflet";

import { EDeficiencyType } from "@/models/enums";
import { IProfileJournalEntry } from "@/models/profile/IProfileJournalEntry";
import { IProfilePhotoHistory } from "@/models/profile/IProfilePhotoHistory";
import { UserAPIService } from "@/shared/services/user-api.service";
import { LanguageService } from "@/shared/services/language.service";

const WATER_COLOR = "#1976d2";
const SOIL_COLOR = "#795548";

function makeMarkerIcon(color: string): L.DivIcon {
  return L.divIcon({
    className: "",
    html: `<div style="width:14px;height:14px;border-radius:50%;background:${color};border:2px solid #fff;box-shadow:0 1px 3px rgba(0,0,0,.4)"></div>`,
    iconSize: [14, 14],
    iconAnchor: [7, 7],
  });
}

@Component({
  selector: "nat-profile-journal-tab",
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule, MatCardModule, MatIconModule, MatTableModule, MatButtonModule, MatButtonToggleModule],
  templateUrl: "./profile-journal-tab.component.html",
  styleUrls: ["./profile-journal-tab.component.css"],
})
export class ProfileJournalTabComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild("mapContainer", { static: false }) mapContainerRef!: ElementRef<HTMLElement>;

  readonly displayedColumns = ["type", "title", "createdOn", "address", "link"];
  journal: IProfileJournalEntry[] = [];
  photos: IProfilePhotoHistory[] = [];
  activeFilter: number | null = null;
  readonly deficiencyTypeEnum = EDeficiencyType;

  private destroy$ = new Subject<void>();
  private map: L.Map | null = null;
  private markersLayer: L.LayerGroup = L.layerGroup();

  constructor(
    private userApi: UserAPIService,
    private languageService: LanguageService,
    private zone: NgZone,
  ) {}

  ngOnInit(): void {
    this.loadJournal();
    this.userApi
      .getProfilePhotoHistory()
      .pipe(takeUntil(this.destroy$))
      .subscribe((p) => {
        this.photos = p;
      });
  }

  ngAfterViewInit(): void {
    this.zone.runOutsideAngular(() => {
      setTimeout(() => this.initMap(), 200);
    });
  }

  ngOnDestroy(): void {
    this.map?.remove();
    this.map = null;
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadJournal(): void {
    this.userApi
      .getProfileJournal(200, 0, this.activeFilter ?? undefined)
      .pipe(takeUntil(this.destroy$))
      .subscribe((j) => {
        this.journal = j;
        this.refreshMarkers();
      });
  }

  onFilterChange(value: number | null): void {
    this.activeFilter = value;
    this.loadJournal();
  }

  deficiencyPath(type: number): string {
    return type === EDeficiencyType.Soil ? "soil" : "water";
  }

  deficiencyLinkCommands(type: number, id: string): string[] {
    const segment = this.deficiencyPath(type);
    const lang = this.languageService.getLanguageFromUrl();
    if (lang === "uk") return ["/", segment, id];
    return ["/", lang, segment, id];
  }

  private initMap(): void {
    const el = this.mapContainerRef?.nativeElement;
    if (!el || this.map) return;
    this.map = L.map(el, { zoomControl: true, scrollWheelZoom: false }).setView([48.5, 31.5], 5);
    L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", { attribution: "OpenStreetMap" }).addTo(this.map);
    this.markersLayer.addTo(this.map);
    this.refreshMarkers();
  }

  private refreshMarkers(): void {
    if (!this.map) return;
    this.markersLayer.clearLayers();
    const bounds: L.LatLng[] = [];
    for (const entry of this.journal) {
      if (entry.latitude == null || entry.longitude == null) continue;
      const color = entry.deficiencyType === EDeficiencyType.Soil ? SOIL_COLOR : WATER_COLOR;
      const marker = L.marker([entry.latitude, entry.longitude], { icon: makeMarkerIcon(color) });
      marker.bindPopup(`<strong>${entry.title}</strong>`);
      this.markersLayer.addLayer(marker);
      bounds.push(L.latLng(entry.latitude, entry.longitude));
    }
    if (bounds.length > 0) {
      this.map.fitBounds(L.latLngBounds(bounds), { padding: [40, 40], maxZoom: 12 });
    }
  }
}
