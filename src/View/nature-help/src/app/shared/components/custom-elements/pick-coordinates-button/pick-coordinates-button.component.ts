import { Component, EventEmitter, Input, Output, HostListener, ElementRef } from "@angular/core";
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";
import { MatTooltipModule } from "@angular/material/tooltip";

@Component({
  selector: "nat-pick-coordinates-button",
  standalone: true,
  imports: [MatButtonModule, MatIconModule, MatTooltipModule],
  templateUrl: "./pick-coordinates-button.component.html",
  styleUrls: ["./pick-coordinates-button.component.css"],
})
export class PickCoordinatesButtonComponent {
  @Input() isActive: boolean = false;
  @Output() buttonClick = new EventEmitter<MouseEvent>();

  constructor(private elementRef: ElementRef) {}

  onButtonClick(event: MouseEvent): void {
    event.stopPropagation();
    this.buttonClick.emit(event);
  }

  @HostListener("document:click", ["$event"])
  onDocumentClick(event: MouseEvent): void {
    if (this.isActive && !this.elementRef.nativeElement.contains(event.target)) {
      this.buttonClick.emit(event);
    }
  }
}
