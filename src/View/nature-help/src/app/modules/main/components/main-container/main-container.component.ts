import { Component, OnInit } from "@angular/core";

import { LoadingService } from "@/shared/services/loading.service";
import { NavigationEnd, Router } from "@angular/router";

@Component({
  selector: "n-main-container",
  templateUrl: "./main-container.component.html",
  styleUrls: ["./main-container.component.css"],
  standalone: false,
})
export class MainContainerComponent implements OnInit {
  isLeftSideVisible = true;

  constructor(
    public loading: LoadingService,
    private router: Router,
  ) {
    const isMobile = /iPhone|iPad|iPod|Android|webOS|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.isLeftSideVisible = !["/owner"].includes(event.url) && !isMobile;
      }
    });
  }

  ngOnInit(): void {}
}
