import { Router } from '@angular/router';
import { WaterAPIService } from '@/modules/water-deficiency/services/water-api.service';
import { SoilAPIService } from '@/modules/soil-deficiency/services/soil-api.service';
import { MapViewService } from '@/shared/services/map-view.service';
import { Component, OnInit, HostListener } from '@angular/core';

@Component({
  selector: 'nat-map',
  templateUrl: './main-map.component.html',
  styleUrls: ['./main-map.component.css'],
  standalone: false,
})
export class MapComponent implements OnInit {
  isFullScreen: boolean = false;

  constructor(
    private waterDataService: WaterAPIService,
    private soilDataService: SoilAPIService,
    private mapViewService: MapViewService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.mapViewService.initMap();
  }

  public goToFullScreen(): void {
    const mapElement = document.getElementById('map');
    if (!mapElement) return;

    if (!this.isFullScreen) {
      this.enterFullScreen(mapElement);
    } else {
      this.exitFullScreen(mapElement);
    }
  }

  private enterFullScreen(mapElement: HTMLElement): void {
    if (mapElement.requestFullscreen) {
      mapElement.requestFullscreen();
    } else if ((mapElement as any).webkitRequestFullscreen) {
      (mapElement as any).webkitRequestFullscreen();
    } else if ((mapElement as any).mozRequestFullScreen) {
      (mapElement as any).mozRequestFullScreen();
    } else if ((mapElement as any).msRequestFullscreen) {
      (mapElement as any).msRequestFullscreen();
    }

    mapElement.style.width = '100vw';
    mapElement.style.height = '100vh';
    this.isFullScreen = true;
  }

  private exitFullScreen(mapElement: HTMLElement): void {
    if (document.exitFullscreen) {
      document.exitFullscreen();
    } else if ((document as any).webkitExitFullscreen) {
      (document as any).webkitExitFullscreen();
    } else if ((document as any).mozCancelFullScreen) {
      (document as any).mozCancelFullScreen();
    } else if ((document as any).msExitFullscreen) {
      (document as any).msExitFullscreen();
    }

    mapElement.style.width = '100%';
    mapElement.style.height = '100%';
    this.isFullScreen = false;
  }

  @HostListener('document:keydown', ['$event'])
  onKeydown(event: KeyboardEvent): void {
    if (event.key === 'Escape' && this.isFullScreen) {
      const mapElement = document.getElementById('map');
      if (mapElement) {
        this.exitFullScreen(mapElement);
        setTimeout(() => {
          this.exitFullScreen(mapElement);
        }, 0);
      }
    }
  }
}
