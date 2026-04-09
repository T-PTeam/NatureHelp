import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { LanguageService } from "../../services/language.service";

@Component({
  selector: "app-footer",
  templateUrl: "./footer.component.html",
  styleUrls: ["./footer.component.css"],
  standalone: false,
})
export class FooterComponent implements OnInit {
  currentYear: number;

  constructor(
    private router: Router,
    private languageService: LanguageService,
  ) {
    this.currentYear = new Date().getFullYear();
  }

  ngOnInit(): void {}

  navigateToPrivacy(): void {
    const currentLang = this.languageService.getCurrentLanguage();
    this.router.navigate([`/${currentLang}/privacy`]);
  }

  navigateToContacts(): void {
    const currentLang = this.languageService.getCurrentLanguage();
    this.router.navigate([`/${currentLang}/about`]);
  }
}
