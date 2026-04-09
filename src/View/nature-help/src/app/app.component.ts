import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { UserAPIService } from "./shared/services/user-api.service";
import { AuditService } from "./shared/services/audit.service";
import { LanguageService } from "./shared/services/language.service";

@Component({
  selector: "n-root",
  templateUrl: "./app.component.html",
  styleUrl: "./app.component.css",
  standalone: false,
})
export class AppComponent implements OnInit {
  constructor(
    private userService: UserAPIService,
    private auditService: AuditService,
    private languageService: LanguageService,
    private route: ActivatedRoute,
    private router: Router,
  ) {}

  ngOnInit() {
    this.languageService.initializeLanguage();

    setTimeout(() => {
      const currentLang = this.languageService.getCurrentLanguage();
    }, 100);

    this.handleOAuth2Redirect();

    this.userService.initializeAuth().subscribe((isLoggedIn) => {
      if (isLoggedIn) {
        const monitoringScheme = this.userService.getCurrentUserMonitoringScheme();
        if (monitoringScheme) {
          this.auditService.setMonitoringScheme(monitoringScheme);
        }
      }
    });

    this.userService.$user.subscribe((user) => {
      if (user?.deficiencyMonitoringScheme) {
        this.auditService.setMonitoringScheme(user.deficiencyMonitoringScheme);
      }
    });
  }

  private handleOAuth2Redirect(): void {
    this.route.queryParams.subscribe((params) => {
      if (params["oauth"]) {
        const provider = params["oauth"];
        setTimeout(() => {
          this.userService.handleOAuth2Callback().subscribe((isLoggedIn) => {
            if (isLoggedIn) {
              this.router.navigate(["/"], { replaceUrl: true, queryParams: {} });
            } else {
              this.router.navigate(["/"], { replaceUrl: true, queryParams: { error: "oauth_failed" } });
            }
          });
        }, 500);
      } else if (params["error"]) {
        const error = params["error"];
        console.error("OAuth2 error:", error);
        this.router.navigate(["/"], { replaceUrl: true, queryParams: {} });
      }
    });
  }
}
