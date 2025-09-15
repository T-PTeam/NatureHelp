import { Component, EventEmitter, Input, Output } from "@angular/core";
import { MatButtonToggleChange, MatButtonToggleModule } from "@angular/material/button-toggle";
import { MatIconModule } from "@angular/material/icon";

@Component({
  selector: "app-water-soil-toggle",
  standalone: true,
  imports: [MatButtonToggleModule, MatIconModule],
  templateUrl: "./water-soil-toggle.component.html",
  styleUrls: ["./water-soil-toggle.component.css"],
})
export class WaterSoilToggleComponent {
  @Input() isWaterSelected: boolean = true;
  @Output() toggleChange = new EventEmitter<boolean>();

  get selectedType(): "water" | "soil" {
    return this.isWaterSelected ? "water" : "soil";
  }

  onTypeChange(event: MatButtonToggleChange) {
    if (event.value) {
      const isWater = event.value === "water";
      this.toggleChange.emit(isWater);
    }
  }
}
