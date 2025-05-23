import "@angular/localize/init";
import { mount } from "cypress/angular";

import { CommonModule } from "@angular/common";
import { ReactiveFormsModule, FormsModule } from "@angular/forms";

// Angular Material
import { MatTableModule } from "@angular/material/table";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { MatTooltipModule } from "@angular/material/tooltip";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";

// Services (mock or real)
import { WaterAPIService } from "@/modules/water-deficiency/services/water-api.service";
import { LoadingService } from "@/shared/services/loading.service";
import { ReportAPIService } from "@/shared/services/report-api.service";
import { SoilAPIService } from "@/modules/soil-deficiency/services/soil-api.service";

import { LabsTableComponent } from "./labs-table.component";
import { HttpClient } from "@angular/common/http";
import { HttpClientModule } from "@angular/common/http";

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
        HttpClientModule,
      ],
      providers: [WaterAPIService, LoadingService, ReportAPIService, SoilAPIService, HttpClient],
      declarations: [],
    });

    cy.get(".grid-container").should("be.visible");
  });
});
