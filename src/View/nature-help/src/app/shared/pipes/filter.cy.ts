import { EDangerState, EDeficiencyType } from "@/models/enums";
import { FilterPipe } from "./filter.pipe";
import { IWaterDeficiency } from "@/modules/water-deficiency/models/IWaterDeficiency";
import { IUser } from "@/models/IUser";
import { ILocation } from "@/models/ILocation";

describe("FilterPipe", () => {
  let pipe: FilterPipe;

  const fixedDate = new Date("2024-01-01T00:00:00Z");

  const defaultUser: IUser = {
    id: "default-id",
    firstName: "Default",
    lastName: "User",
    email: "default@example.com",
    password: "",
    passwordHash: "",
    role: 0,
    organizationId: null,
    address: null,
  };

  const defaultLocation: ILocation = {
    id: "default-location-id",
    country: "Unknown",
    region: "Unknown",
    district: "Unknown",
    city: "Unknown",
    latitude: 0,
    longitude: 0,
    radiusAffected: 0,
  };

  const mockData: IWaterDeficiency[] = [
    {
      title: "High Lead",
      description: "",
      eDangerState: EDangerState.Dangerous,
      ph: 0,
      dissolvedOxygen: 0,
      leadConcentration: 0,
      mercuryConcentration: 0,
      nitratesConcentration: 0,
      pesticidesContent: 0,
      microbialActivity: 0,
      radiationLevel: 0,
      chemicalOxygenDemand: 0,
      biologicalOxygenDemand: 0,
      phosphateConcentration: 0,
      cadmiumConcentration: 0,
      totalDissolvedSolids: 0,
      electricalConductivity: 0,
      microbialLoad: 0,
      type: EDeficiencyType.Water,
      creator: defaultUser,
      location: defaultLocation,
      changedModelLogId: "",
      changedModelLog: [],
      id: "",
      createdBy: defaultUser,
      createdOn: fixedDate,
    },
    {
      title: "Low Oxygen",
      description: "",
      eDangerState: EDangerState.Moderate,
      ph: 0,
      dissolvedOxygen: 0,
      leadConcentration: 0,
      mercuryConcentration: 0,
      nitratesConcentration: 0,
      pesticidesContent: 0,
      microbialActivity: 0,
      radiationLevel: 0,
      chemicalOxygenDemand: 0,
      biologicalOxygenDemand: 0,
      phosphateConcentration: 0,
      cadmiumConcentration: 0,
      totalDissolvedSolids: 0,
      electricalConductivity: 0,
      microbialLoad: 0,
      type: EDeficiencyType.Water,
      creator: defaultUser,
      location: defaultLocation,
      changedModelLogId: "",
      changedModelLog: [],
      id: "",
      createdBy: defaultUser,
      createdOn: fixedDate,
    },
    {
      title: "Chemical Pollution",
      description: "",
      eDangerState: EDangerState.Critical,
      ph: 0,
      dissolvedOxygen: 0,
      leadConcentration: 0,
      mercuryConcentration: 0,
      nitratesConcentration: 0,
      pesticidesContent: 0,
      microbialActivity: 0,
      radiationLevel: 0,
      chemicalOxygenDemand: 0,
      biologicalOxygenDemand: 0,
      phosphateConcentration: 0,
      cadmiumConcentration: 0,
      totalDissolvedSolids: 0,
      electricalConductivity: 0,
      microbialLoad: 0,
      type: EDeficiencyType.Water,
      creator: defaultUser,
      location: defaultLocation,
      changedModelLogId: "",
      changedModelLog: [],
      id: "",
      createdBy: defaultUser,
      createdOn: fixedDate,
    },
  ];

  beforeEach(() => {
    pipe = new FilterPipe();
  });

  it("should filter water deficiencies correctly", () => {
    const result = pipe.transform(mockData, "Lead");
    expect(result).to.have.length(1);
    expect(result[0].title).to.equal("High Lead");
    expect(result[0].eDangerState).to.equal(EDangerState.Dangerous);
  });

  it("should return all deficiencies if search value is empty", () => {
    const result = pipe.transform(mockData, "");
    expect(result).to.deep.equal(mockData);
  });

  it("should return an empty array if no match is found", () => {
    const result = pipe.transform(mockData, "Mercury");
    expect(result).to.deep.equal([]);
  });
});
