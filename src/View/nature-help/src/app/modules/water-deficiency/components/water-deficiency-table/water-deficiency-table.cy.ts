import "@angular/localize/init";
import { mount } from "cypress/angular";
import { WaterDeficiencyTable } from "./water-deficiency-table.component";

import { CommonModule } from "@angular/common";
import { ReactiveFormsModule, FormsModule } from "@angular/forms";

import { MatTableModule } from "@angular/material/table";
import { MatFormFieldControl, MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { MatTooltipModule } from "@angular/material/tooltip";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";

import { WaterAPIService } from "@/modules/water-deficiency/services/water-api.service";
import { LoadingService } from "@/shared/services/loading.service";
import { ReportAPIService } from "@/shared/services/report-api.service";
import { SoilAPIService } from "@/modules/soil-deficiency/services/soil-api.service";
import { LabResearchersPipe } from "@/shared/pipes/lab-researchers.pipe";
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { EnumToStringPipe } from "@/shared/pipes/enum-to-string.pipe";

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
        HttpClientModule,
        MatFormFieldControl,
      ],
      providers: [WaterAPIService, LoadingService, ReportAPIService, SoilAPIService, HttpClient],
      declarations: [LabResearchersPipe, EnumToStringPipe],
    });

    cy.get(".grid-container").should("be.visible");
  });
});
