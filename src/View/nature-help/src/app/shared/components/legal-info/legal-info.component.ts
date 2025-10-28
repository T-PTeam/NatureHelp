import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-legal-info",
  templateUrl: "./legal-info.component.html",
  styleUrls: ["./legal-info.component.css"],
  standalone: false,
})
export class LegalInfoComponent implements OnInit {
  currentYear: number;

  constructor() {
    this.currentYear = new Date().getFullYear();
  }

  ngOnInit(): void {}
}
