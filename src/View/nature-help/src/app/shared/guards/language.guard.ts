import { Injectable } from "@angular/core";
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { LanguageService } from "../services/language.service";

@Injectable({
  providedIn: "root",
})
export class LanguageGuard implements CanActivate {
  constructor(
    private languageService: LanguageService,
    private router: Router,
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const url = state.url;
    const urlSegments = url.split("/").filter((segment) => segment !== "");

    if (urlSegments.length > 0 && this.languageService.getSupportedLanguages().includes(urlSegments[0])) {
      const language = urlSegments[0];
      this.languageService.setLanguage(language);
      return true;
    }

    const savedLanguage = localStorage.getItem("selectedLanguage");
    const browserLanguage = navigator.language.split("-")[0];

    let targetLanguage = savedLanguage || (browserLanguage === "en" ? "en" : "uk");

    if (!this.languageService.getSupportedLanguages().includes(targetLanguage)) {
      targetLanguage = "uk";
    }

    const newUrl = targetLanguage === "uk" ? url : `/${targetLanguage}${url}`;
    this.router.navigateByUrl(newUrl, { replaceUrl: true });

    return false;
  }
}
