import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'n-main-container',
  templateUrl: './main-container.component.html',
  styleUrls: ['./main-container.component.css'],
  standalone: false,
})
export class MainContainerComponent implements OnInit {
  showWaterTable: boolean = true;
  showSwitch: boolean = false;

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.updateTableState(this.router.url);
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.updateTableState(event.urlAfterRedirects);
      }
    });
  }

  updateTableState(currentUrl: string): void {
    if (['/', '/water', '/soil'].includes(currentUrl)) {
      this.showSwitch = true; 
    } else {
      this.showSwitch = false;
    }

    if (currentUrl.includes('soil')) {
      this.showWaterTable = false;
    } else if (currentUrl.includes('water')) {
      this.showWaterTable = true;
    } else if (currentUrl === '/') {
      this.showWaterTable = true;
    }
  }

  onToggleSwitch(value: boolean): void {
    this.showWaterTable = value;
    this.router.navigate([value ? 'water' : 'soil']);
  }
}
