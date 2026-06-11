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
  loadingVisible = false;
  private routerSubscription?: Subscription;
  private loadingSubscription?: Subscription;

  constructor(
    public loading: LoadingService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    const isMobile = /iPhone|iPad|iPod|Android|webOS|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
    this.isLeftSideVisible = !this.router.url.includes("/owner") && !isMobile;

    this.loadingSubscription = this.loading.loading$.subscribe((visible) => {
      setTimeout(() => {
        this.loadingVisible = visible;
      });
    });

    this.routerSubscription = this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        const visible = !event.url.includes("/owner") && !isMobile;
        if (this.isLeftSideVisible !== visible) {
          setTimeout(() => {
            this.isLeftSideVisible = visible;
          });
        }
      }
    });
  }

  ngOnDestroy(): void {
    this.routerSubscription?.unsubscribe();
    this.loadingSubscription?.unsubscribe();
  }
}
