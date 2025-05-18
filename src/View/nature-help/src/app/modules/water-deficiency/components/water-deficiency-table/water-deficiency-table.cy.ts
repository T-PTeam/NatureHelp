import "@angular/localize/init";
import { mount } from "cypress/angular";
import { WaterDeficiencyTable } from "./water-deficiency-table.component";
import { WaterAPIService } from "@/modules/water-deficiency/services/water-api.service";
import { LoadingService } from "@/shared/services/loading.service";
import { HttpClientModule } from "@angular/common/http";
import { ReportAPIService } from "@/shared/services/report-api.service";
import { SoilAPIService } from "@/modules/soil-deficiency/services/soil-api.service";

describe("WaterDeficiencyTable Component", () => {
  it("renders correctly", () => {
    mount(WaterDeficiencyTable, {
      imports: [HttpClientModule], // Provide HttpClientModule
      providers: [WaterAPIService, LoadingService, ReportAPIService, SoilAPIService], // Inject services manually
    });
    cy.get(".grid-container").should("be.visible");
  });
});
