import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import moment from "moment";

@Injectable({
  providedIn: "root",
})
export class LanguageService {
  private readonly defaultLanguage = "uk";
  private readonly supportedLanguages = ["uk", "en"];

  constructor(
    private translate: TranslateService,
    private router: Router,
  ) {
    this.translate.setDefaultLang(this.defaultLanguage);

    this.initializeLanguageFromUrl();
  }

  private initializeLanguageFromUrl(): void {
    const currentUrl = this.router.url;
    const urlSegments = currentUrl.split("/").filter((segment) => segment !== "");

    let language = this.defaultLanguage;
    if (urlSegments.length > 0 && this.supportedLanguages.includes(urlSegments[0])) {
      language = urlSegments[0];
    } else {
      language = localStorage.getItem("selectedLanguage") || this.defaultLanguage;
    }

    this.setLanguage(language);
  }

  initializeLanguage(): void {
    const savedLanguage = localStorage.getItem("selectedLanguage") || this.defaultLanguage;
    this.setLanguage(savedLanguage);
  }

  setLanguage(languageCode: string): void {
    if (!this.supportedLanguages.includes(languageCode)) {
      languageCode = this.defaultLanguage;
    }

    this.translate.use(languageCode).subscribe(() => {});

    localStorage.setItem("selectedLanguage", languageCode);

    moment.locale(languageCode === "en" ? "en" : "uk");

    this.updateUrl(languageCode);
  }

  getCurrentLanguage(): string {
    return this.translate.currentLang || this.defaultLanguage;
  }

  getSupportedLanguages(): string[] {
    return this.supportedLanguages;
  }

  private updateUrl(languageCode: string): void {
    const currentUrl = this.router.url;
    const urlSegments = currentUrl.split("/").filter((segment) => segment !== "");

    if (urlSegments.length > 0 && this.supportedLanguages.includes(urlSegments[0])) {
      urlSegments.splice(0, 1);
    }

    if (languageCode !== this.defaultLanguage) {
      urlSegments.unshift(languageCode);
    }

    const newUrl = "/" + urlSegments.join("/");

    this.router.navigateByUrl(newUrl, { replaceUrl: true });
  }

  getLanguageFromUrl(): string {
    const urlSegments = this.router.url.split("/");
    const firstSegment = urlSegments[1];

    if (this.supportedLanguages.includes(firstSegment)) {
      return firstSegment;
    }

    return this.defaultLanguage;
  }

  isLanguageInUrl(): boolean {
    const urlSegments = this.router.url.split("/");
    return this.supportedLanguages.includes(urlSegments[1]);
  }
}
