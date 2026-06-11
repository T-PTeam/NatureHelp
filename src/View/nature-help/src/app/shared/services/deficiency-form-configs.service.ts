import { Validators } from "@angular/forms";
import moment from "moment";

import { EDeficiencyType } from "@/models/enums";
import { IUser } from "@/models/IUser";
import { IWaterDeficiency } from "@/modules/water-deficiency/models/IWaterDeficiency";
import { ISoilDeficiency } from "@/modules/soil-deficiency/models/ISoilDeficiency";
import { IDeficiencyFormConfig } from "@/shared/models/IDeficiencyFormConfig";

export class WaterDeficiencyFormConfig implements IDeficiencyFormConfig {
  deficiencyType = EDeficiencyType.Water;

  researchFieldNames = [
    "ph",
    "dissolvedOxygen",
    "leadConcentration",
    "mercuryConcentration",
    "nitrateConcentration",
    "pesticidesContent",
    "microbialActivity",
    "radiationLevel",
    "chemicalOxygenDemand",
    "biologicalOxygenDemand",
    "phosphateConcentration",
    "cadmiumConcentration",
    "totalDissolvedSolids",
    "electricalConductivity",
    "microbialLoad",
  ];

  getSpecificFormFields(deficiency: IWaterDeficiency | null, currentUser: IUser | null): any {
    return {
      ph: [deficiency?.ph || 0, [Validators.required, Validators.min(0), Validators.max(14)]],
      dissolvedOxygen: [deficiency?.dissolvedOxygen || 0, [Validators.required, Validators.min(0), Validators.max(20)]],
      leadConcentration: [
        deficiency?.leadConcentration || 0,
        [Validators.required, Validators.min(0), Validators.max(0.02)],
      ],
      mercuryConcentration: [
        deficiency?.mercuryConcentration || 0,
        [Validators.required, Validators.min(0), Validators.max(0.01)],
      ],
      nitrateConcentration: [
        deficiency?.nitrateConcentration || 0,
        [Validators.required, Validators.min(0), Validators.max(50)],
      ],
      pesticidesContent: [
        deficiency?.pesticidesContent || 0,
        [Validators.required, Validators.min(0), Validators.max(0.1)],
      ],
      microbialActivity: [
        deficiency?.microbialActivity || 0,
        [Validators.required, Validators.min(0), Validators.max(1000)],
      ],
      radiationLevel: [deficiency?.radiationLevel || 0, [Validators.required, Validators.min(0), Validators.max(50)]],
      chemicalOxygenDemand: [
        deficiency?.chemicalOxygenDemand || 0,
        [Validators.required, Validators.min(0), Validators.max(1000)],
      ],
      biologicalOxygenDemand: [
        deficiency?.biologicalOxygenDemand || 0,
        [Validators.required, Validators.min(0), Validators.max(50)],
      ],
      phosphateConcentration: [
        deficiency?.phosphateConcentration || 0,
        [Validators.required, Validators.min(0), Validators.max(10)],
      ],
      cadmiumConcentration: [
        deficiency?.cadmiumConcentration || 0,
        [Validators.required, Validators.min(0), Validators.max(0.01)],
      ],
      totalDissolvedSolids: [
        deficiency?.totalDissolvedSolids || 0,
        [Validators.required, Validators.min(0), Validators.max(1500)],
      ],
      electricalConductivity: [
        deficiency?.electricalConductivity || 0,
        [Validators.required, Validators.min(0), Validators.max(2500)],
      ],
      microbialLoad: [deficiency?.microbialLoad || 0, [Validators.required, Validators.min(0), Validators.max(5000)]],
    };
  }
}

export class SoilDeficiencyFormConfig implements IDeficiencyFormConfig {
  deficiencyType = EDeficiencyType.Soil;

  researchFieldNames = [
    "ph",
    "organicMatter",
    "leadConcentration",
    "cadmiumConcentration",
    "mercuryConcentration",
    "pesticidesContent",
    "nitrateConcentration",
    "heavyMetalsConcentration",
    "electricalConductivity",
    "microbialActivity",
    "analysisDate",
  ];

  getSpecificFormFields(deficiency: ISoilDeficiency | null, currentUser: IUser | null): any {
    return {
      ph: [deficiency?.ph ?? 6.5, [Validators.required, Validators.min(0)]],
      organicMatter: [deficiency?.organicMatter || 0, [Validators.required, Validators.min(0)]],
      leadConcentration: [deficiency?.leadConcentration || 0, [Validators.required, Validators.min(0)]],
      cadmiumConcentration: [deficiency?.cadmiumConcentration || 0, [Validators.required, Validators.min(0)]],
      mercuryConcentration: [deficiency?.mercuryConcentration || 0, [Validators.required, Validators.min(0)]],
      pesticidesContent: [deficiency?.pesticidesContent || 0, [Validators.required, Validators.min(0)]],
      nitrateConcentration: [deficiency?.nitrateConcentration || 0, [Validators.required, Validators.min(0)]],
      heavyMetalsConcentration: [deficiency?.heavyMetalsConcentration || 0, [Validators.required, Validators.min(0)]],
      electricalConductivity: [deficiency?.electricalConductivity || 0, [Validators.required, Validators.min(0)]],
      microbialActivity: [deficiency?.microbialActivity || 0, [Validators.required, Validators.min(0)]],
      analysisDate: [deficiency?.analysisDate || moment()],
    };
  }
}
