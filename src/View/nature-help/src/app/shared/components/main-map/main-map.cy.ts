import { MapViewService } from "@/shared/services/map-view.service";
import { MapComponent } from "./main-map.component";
import { SoilAPIService } from "@/modules/soil-deficiency/services/soil-api.service";
import { WaterAPIService } from "@/modules/water-deficiency/services/water-api.service";
import { LoadingService } from "@/shared/services/loading.service";
import { HttpClient, HttpClientModule, HttpHandler } from "@angular/common/http";
import { mount } from "cypress/angular";

describe("MapComponent", () => {
  beforeEach(() => {
    mount(MapComponent, {
      imports: [HttpClientModule],
      providers: [MapViewService, WaterAPIService, HttpClient, HttpHandler, LoadingService, SoilAPIService], // Mock services
    });
  });

  // ✅ Test to check if the map loads correctly
  it("should initialize the map", () => {
    cy.get("#map").should("exist");
  });

  // ✅ Test to ensure markers are placed on the map
  it("should create deficiency and lab markers", () => {
    cy.spy(MapViewService.prototype, "makeDeficiencyMarkers").as("deficiencyMarkers");
    cy.spy(MapViewService.prototype, "makeLabMarkers").as("labMarkers");

    mount(MapComponent);

    cy.get("@deficiencyMarkers").should("have.been.called");
    cy.get("@labMarkers").should("have.been.called");
  });

  // ✅ Test to check if a button exists for zoom interaction
  it("should contain a zoom-in button", () => {
    cy.get(".zoom-in-button", { timeout: 5000 }).should("exist");
  });

  // ✅ Test for checking navigation controls
  it("should have navigation controls", () => {
    cy.get(".navigation-controls").should("exist");
  });

  // ✅ Test to verify map service initialization
  it("should call map initialization service on load", () => {
    cy.spy(MapViewService.prototype, "initMap").as("initMap");

    mount(MapComponent);

    cy.get("@initMap").should("have.been.called");
  });
});
