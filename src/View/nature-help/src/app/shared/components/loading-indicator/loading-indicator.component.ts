import { Component, Input } from "@angular/core";

@Component({
  selector: "nat-loading-indicator",
  templateUrl: "./loading-indicator.component.html",
  styleUrls: ["./loading-indicator.component.css"],
  standalone: false,
})
export class LoadingIndicatorComponent {
  @Input() type: "overlay" | "inline" | "button" | "progress" | "more" = "inline";
  @Input() diameter: number = 50;
  @Input() message: string = "";
  @Input() progress: number = 0;
  @Input() showMessage: boolean = true;
}

