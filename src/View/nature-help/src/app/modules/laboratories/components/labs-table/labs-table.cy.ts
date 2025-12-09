import { mount } from "cypress/angular";

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

import { LabsTableComponent } from "./labs-table.component";
import { HttpClient } from "@angular/common/http";
import { HttpClientModule } from "@angular/common/http";
import { LoadingIndicatorComponent } from "@/shared/components/loading-indicator/loading-indicator.component";
import { TranslateModule, TranslateService } from "@ngx-translate/core";
import { TranslateHttpLoader } from "@ngx-translate/http-loader";
import { TranslateLoader } from "@ngx-translate/core";
import { LabsAPIService } from "@/modules/laboratories/services/labs-api.service";
import { Router } from "@angular/router";

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, "./assets/i18n/", ".json");
}

describe("LabsTable Component", () => {
  it("renders correctly", () => {
    mount(LabsTableComponent, {
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
      declarations: [LoadingIndicatorComponent],
      providers: [
        WaterAPIService,
        LoadingService,
        ReportAPIService,
        SoilAPIService,
        HttpClient,
        TranslateService,
        { provide: LabsAPIService, useValue: {} },
        { provide: Router, useValue: { navigate: cy.stub() } },
      ],
    });

    cy.get(".grid-container").should("be.visible");
  });
});
