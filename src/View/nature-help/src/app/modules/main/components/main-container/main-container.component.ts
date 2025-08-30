import { Component, OnInit, OnDestroy } from "@angular/core";

import { LoadingService } from "@/shared/services/loading.service";
import { NavigationEnd, Router } from "@angular/router";
import { Subscription } from "rxjs";

@Component({
  selector: "n-main-container",
  templateUrl: "./main-container.component.html",
  styleUrls: ["./main-container.component.css"],
  standalone: false,
})
export class MainContainerComponent implements OnInit, OnDestroy {
  isLeftSideVisible = true;
  private routerSubscription?: Subscription;

  constructor(
    public loading: LoadingService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    const isMobile = /iPhone|iPad|iPod|Android|webOS|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
    this.routerSubscription = this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.isLeftSideVisible = !["/owner"].includes(event.url) && !isMobile;
      }
    });
  }

  ngOnDestroy(): void {
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }
  }
}
