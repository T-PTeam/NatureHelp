import { Component, OnInit } from "@angular/core";
import { LanguageService } from "../../services/language.service";

@Component({
  selector: "app-language-switcher",
  templateUrl: "./language-switcher.component.html",
  styleUrls: ["./language-switcher.component.css"],
  standalone: false,
})
export class LanguageSwitcherComponent implements OnInit {
  currentLanguage: string = "uk";
  languages = [
    { code: "uk", name: "Українська", flag: "🇺🇦" },
    { code: "en", name: "English", flag: "🇺🇸" },
  ];

  constructor(private languageService: LanguageService) {}

  ngOnInit(): void {
    this.currentLanguage = this.languageService.getCurrentLanguage();
  }

  switchLanguage(event: any): void {
    const languageCode = event.target ? event.target.value : event;
    console.log("Switching language to:", languageCode);
    this.currentLanguage = languageCode;

    this.languageService.setLanguage(languageCode);
  }
}
