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
    }
  }

  toggleTable(): void {
    this.showWaterTable = !this.showWaterTable;
    if (this.showWaterTable) {
      this.router.navigate(['water']);
    } else {
      this.router.navigate(['soil']);
    }
  }
}
