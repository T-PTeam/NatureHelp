import { Component, OnInit } from "@angular/core";
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
  ) {}

  ngOnInit() {
    setTimeout(() => {
      this.languageService.initializeLanguage();
    }, 100);

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
}
