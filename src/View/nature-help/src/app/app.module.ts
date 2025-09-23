import "moment/locale/uk";

import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { MAT_DATE_FORMATS, MAT_DATE_LOCALE } from "@angular/material/core";
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS } from "@angular/material-moment-adapter";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { provideAnimationsAsync } from "@angular/platform-browser/animations/async";
import moment from "moment";

import { AppComponent } from "./app.component";
import { MatModule } from "./mat.module";
import { LabsModule } from "./modules/laboratories/labs.module";
import { LabsAPIService } from "./modules/laboratories/services/labs-api.service";
import { MainModule } from "./modules/main/main.module";
import { SoilAPIService } from "./modules/soil-deficiency/services/soil-api.service";
import { SoilDeficiencyModule } from "./modules/soil-deficiency/soil-deficiency.module";
import { WaterAPIService } from "./modules/water-deficiency/services/water-api.service";
import { WaterDeficiencyModule } from "./modules/water-deficiency/water-deficiency.module";
import { AuthInterceptor } from "./shared/services/auth-interceptor.service";
import { MapViewService } from "./shared/services/map-view.service";
import { SharedModule } from "./shared/shared.module";
import { OwneryModule } from "./modules/owner/owner.module";
import { ResearchesAPIService } from "./modules/laboratories/services/researches-api.service";
import { OrganizationManagementModule } from "./modules/organization-management/organization-management.module";

const UKRAINIAN_DATE_FORMATS = {
  parse: {
    dateInput: "L",
  },
  display: {
    dateInput: "L",
    monthYearLabel: "MMMM YYYY",
    dateA11yLabel: "LL",
    monthYearA11yLabel: "MMMM YYYY",
  },
};

@NgModule({
  imports: [
    SharedModule,
    MatModule,
    WaterDeficiencyModule,
    SoilDeficiencyModule,
    MainModule,
    BrowserAnimationsModule,
    LabsModule,
    OwneryModule,
    OrganizationManagementModule,
  ],
  declarations: [AppComponent],
  bootstrap: [AppComponent],
  providers: [
    WaterAPIService,
    SoilAPIService,
    LabsAPIService,
    ResearchesAPIService,

    MapViewService,
    provideAnimationsAsync(),

    { provide: MAT_DATE_LOCALE, useValue: "uk-UA" },
    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } },
    { provide: MAT_DATE_FORMATS, useValue: UKRAINIAN_DATE_FORMATS },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ],
})
export class AppModule {
  constructor() {
    moment.locale("uk");
  }
}
