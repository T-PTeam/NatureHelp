import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class MobileMapService {
  private isMobileMapVisibleSubject = new BehaviorSubject<boolean>(false);
  public isMobileMapVisible$: Observable<boolean> = this.isMobileMapVisibleSubject.asObservable();

  public isMobile(): boolean {
    return (
      /iPhone|iPad|iPod|Android|webOS|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent) ||
      window.innerWidth <= 768
    );
  }

  public showMobileMap(): void {
    this.isMobileMapVisibleSubject.next(true);
  }

  public hideMobileMap(): void {
    this.isMobileMapVisibleSubject.next(false);
  }

  public toggleMobileMap(): void {
    const currentValue = this.isMobileMapVisibleSubject.value;
    this.isMobileMapVisibleSubject.next(!currentValue);
  }
}
