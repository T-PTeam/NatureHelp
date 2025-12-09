import { mount } from "cypress/angular";
import { WaterDeficiencyTable } from "./water-deficiency-table.component";

import { CommonModule } from "@angular/common";
import { ReactiveFormsModule, FormsModule } from "@angular/forms";

import { MatTableModule } from "@angular/material/table";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { MatTooltipModule } from "@angular/material/tooltip";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatExpansionModule } from "@angular/material/expansion";
import { MatSelectModule } from "@angular/material/select";
import { InfiniteScrollDirective } from "ngx-infinite-scroll";

import { WaterAPIService } from "@/modules/water-deficiency/services/water-api.service";
import { LoadingService } from "@/shared/services/loading.service";
import { ReportAPIService } from "@/shared/services/report-api.service";
import { SoilAPIService } from "@/modules/soil-deficiency/services/soil-api.service";
import { LabResearchersPipe } from "@/shared/pipes/lab-researchers.pipe";
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { EnumToStringPipe } from "@/shared/pipes/enum-to-string.pipe";
import { LoadingIndicatorComponent } from "@/shared/components/loading-indicator/loading-indicator.component";
import { TranslateModule, TranslateService } from "@ngx-translate/core";
import { TranslateHttpLoader } from "@ngx-translate/http-loader";
import { TranslateLoader } from "@ngx-translate/core";
import { Router } from "@angular/router";
import { UserAPIService } from "@/shared/services/user-api.service";
import { MapViewService } from "@/shared/services/map-view.service";
import { AuditService } from "@/shared/services/audit.service";
import { of } from "rxjs";

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, "./assets/i18n/", ".json");
}

describe("WaterDeficiencyTable Component", () => {
  it("renders correctly", () => {
    mount(WaterDeficiencyTable, {
      imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        MatTableModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatTooltipModule,
        MatProgressSpinnerModule,
        MatExpansionModule,
        MatSelectModule,
        InfiniteScrollDirective,
        HttpClientModule,
        TranslateModule.forRoot({
          loader: {
            provide: TranslateLoader,
            useFactory: HttpLoaderFactory,
            deps: [HttpClient],
          },
          defaultLanguage: "uk",
        }),
      ],
      declarations: [LabResearchersPipe, EnumToStringPipe, LoadingIndicatorComponent],
      providers: [
        LoadingService,
        { provide: WaterAPIService, useValue: { deficiencies$: of([]), totalCount$: of(0) } },
        { provide: ReportAPIService, useValue: {} },
        { provide: SoilAPIService, useValue: {} },
        HttpClient,
        TranslateService,
        { provide: Router, useValue: { navigate: cy.stub() } },
        { provide: UserAPIService, useValue: { $user: of(null) } },
        { provide: MapViewService, useValue: {} },
        { provide: AuditService, useValue: {} },
      ],
    });

    cy.get(".grid-container").should("be.visible");
  });
});
