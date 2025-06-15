/// <reference types="cypress" />
import { FilterPipe } from "./filter.pipe";
import { IWaterDeficiency } from "@/modules/water-deficiency/models/IWaterDeficiency";
import { EDangerState, EDeficiencyType } from "@/models/enums";
import { IUser } from "@/models/IUser";
import { expect } from "chai";

describe("FilterPipe", () => {
  let pipe: FilterPipe;

  beforeEach(() => {
    pipe = new FilterPipe();
  });

  it("should filter water deficiencies by title", () => {
    const user1: IUser = {
      id: "1",
      firstName: "John",
      lastName: "Doe",
      email: "john@example.com",
      password: "",
      passwordHash: "",
      role: 1,
      organizationId: "1",
    };

    const user2: IUser = {
      id: "2",
      firstName: "Jane",
      lastName: "Smith",
      email: "jane@example.com",
      password: "",
      passwordHash: "",
      role: 1,
      organizationId: "1",
    };

    const deficiencies: IWaterDeficiency[] = [
      {
        id: "1",
        title: "Water Pollution",
        description: "High levels of contaminants",
        type: EDeficiencyType.Water,
        creator: user1,
        latitude: 48.65,
        longitude: 22.26,
        eDangerState: EDangerState.Critical,
        changedModelLogId: "1",
        changedModelLog: [],
        createdBy: user1,
        createdOn: new Date(),
        ph: 7.5,
        dissolvedOxygen: 6.2,
        leadConcentration: 0.01,
        mercuryConcentration: 0.001,
        nitratesConcentration: 10,
        pesticidesContent: 0.5,
        microbialActivity: 0.8,
        radiationLevel: 0.1,
        chemicalOxygenDemand: 20,
        biologicalOxygenDemand: 15,
        phosphateConcentration: 0.3,
        cadmiumConcentration: 0.002,
        totalDissolvedSolids: 500,
        electricalConductivity: 800,
        microbialLoad: 100,
      },
      {
        id: "2",
        title: "Soil Contamination",
        description: "Heavy metal presence",
        type: EDeficiencyType.Soil,
        creator: user2,
        latitude: 48.66,
        longitude: 22.27,
        eDangerState: EDangerState.Dangerous,
        changedModelLogId: "2",
        changedModelLog: [],
        createdBy: user2,
        createdOn: new Date(),
        ph: 6.8,
        dissolvedOxygen: 5.8,
        leadConcentration: 0.02,
        mercuryConcentration: 0.002,
        nitratesConcentration: 12,
        pesticidesContent: 0.6,
        microbialActivity: 0.7,
        radiationLevel: 0.15,
        chemicalOxygenDemand: 25,
        biologicalOxygenDemand: 18,
        phosphateConcentration: 0.4,
        cadmiumConcentration: 0.003,
        totalDissolvedSolids: 550,
        electricalConductivity: 850,
        microbialLoad: 120,
      },
    ];

    const result = pipe.transform(deficiencies, "Water");
    expect(result).to.have.length(1);
    expect(result[0].title).to.equal("Water Pollution");
  });
});
