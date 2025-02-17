import { LoadingService } from '@/shared/services/loading.service';
import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'n-main-container',
  templateUrl: './main-container.component.html',
  styleUrls: ['./main-container.component.css'],
  standalone: false,
})
export class MainContainerComponent implements OnInit {
  constructor(public loading: LoadingService) {}

  ngOnInit(): void {
  }
}
