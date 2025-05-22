import { MapViewService } from "@/shared/services/map-view.service";
import { MapComponent } from "./main-map.component";
import { SoilAPIService } from "@/modules/soil-deficiency/services/soil-api.service";
import { WaterAPIService } from "@/modules/water-deficiency/services/water-api.service";
import { LoadingService } from "@/shared/services/loading.service";
import { HttpClient, HttpClientModule, HttpHandler } from "@angular/common/http";
import { mount } from "cypress/angular";
import { of } from "rxjs";
import { LabsAPIService } from "@/modules/laboratories/services/labs-api.service";
import { EDeficiencyType } from "@/models/enums";

const mockWaterAPIService = {
  getDeficiencies: () =>
    of([
      {
        id: 1,
        title: "Water Deficiency A",
        description: "Low water availability",
        eDeficiencyType: 1,
        eDangerState: 2,
        location: { latitude: 48.65, longitude: 22.26, radiusAffected: 15 },
        creator: { firstName: "John", lastName: "Doe" },
        responsibleUser: { firstName: "Jane", lastName: "Smith" },
        microbialLoad: 5,
      },
    ]),
};

const mockSoilAPIService = {
  getDeficiencies: () =>
    of([
      {
        id: 2,
        title: "Soil Deficiency B",
        description: "Soil contamination detected",
        eDeficiencyType: 2,
        eDangerState: 3,
        location: { latitude: 48.66, longitude: 22.27, radiusAffected: 10 },
        creator: { firstName: "Alice", lastName: "Brown" },
        responsibleUser: null,
      },
    ]),
};

const mockLabsAPIService = {
  labs$: of([
    {
      id: 1,
      title: "Lab 1",
      location: { latitude: 48.67, longitude: 22.28 },
      researchersCount: 3,
      researchers: [
        { firstName: "Researcher", lastName: "One" },
        { firstName: "Researcher", lastName: "Two" },
        { firstName: "Researcher", lastName: "Three" },
      ],
    },
  ]),
};

describe("MapComponent", () => {
  beforeEach(() => {
    cy.spy(MapViewService.prototype, "makeDeficiencyMarkers").as("deficiencyMarkers");
    cy.spy(MapViewService.prototype, "makeLabMarkers").as("labMarkers");
    cy.spy(MapViewService.prototype, "initMap").as("initMap");

    mount(MapComponent, {
      imports: [HttpClientModule],
      providers: [
        MapViewService,
        { provide: WaterAPIService, useValue: mockWaterAPIService },
        { provide: SoilAPIService, useValue: mockSoilAPIService },
        { provide: LabsAPIService, useValue: mockLabsAPIService },
      ],
    });
  });

  it("should initialize the map", () => {
    cy.get("#map").should("exist");
  });

  it("should create deficiency and lab markers", () => {
    cy.get("#map", { timeout: 10000 }).should("exist");
    cy.get(".leaflet-marker-icon", { timeout: 10000 }).should("have.length.at.least", 1);

    cy.get("@deficiencyMarkers").should("have.been.called");
    cy.get("@labMarkers").should("have.been.called");
  });

  it("should call map initialization service on load", () => {
    cy.get("@initMap").should("have.been.called");
  });
});
