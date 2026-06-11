import { Component, OnInit, OnDestroy } from "@angular/core";
import { Router, NavigationEnd } from "@angular/router";
import { filter } from "rxjs/operators";
import { Subscription } from "rxjs";
import { TranslateService } from "@ngx-translate/core";
import { LanguageService } from "../../services/language.service";

@Component({
  selector: "app-language-switcher",
  templateUrl: "./language-switcher.component.html",
  styleUrls: ["./language-switcher.component.css"],
  standalone: false,
})
export class LanguageSwitcherComponent implements OnInit, OnDestroy {
  currentLanguage: string = "uk";
  languages = [
    { code: "uk", name: "Українська", flag: "🇺🇦", countryCode: "UA" },
    { code: "en", name: "English", flag: "🇺🇸", countryCode: "US" },
  ];
  private routerSubscription: Subscription = new Subscription();
  private translateSubscription: Subscription = new Subscription();

  constructor(
    private languageService: LanguageService,
    private router: Router,
    private translate: TranslateService,
  ) {}

  ngOnInit(): void {
    this.updateCurrentLanguage();

    this.routerSubscription = this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe(() => {
        this.updateCurrentLanguage();
      });

    this.translateSubscription = this.translate.onLangChange.subscribe((event) => {
      this.currentLanguage = event.lang;
    });
  }

  ngOnDestroy(): void {
    this.routerSubscription.unsubscribe();
    this.translateSubscription.unsubscribe();
  }

  private updateCurrentLanguage(): void {
    const translateLang = this.translate.currentLang;
    const urlLang = this.languageService.getLanguageFromUrl();
    this.currentLanguage = translateLang || urlLang || this.languageService.getCurrentLanguage();
  }

  switchLanguage(languageCode: string): void {
    this.currentLanguage = languageCode;
    this.languageService.setLanguage(languageCode);
  }

  getCurrentFlag(): string {
    const currentLang = this.languages.find((lang) => lang.code === this.currentLanguage);
    return currentLang ? currentLang.flag : "🇺🇦";
  }
}
