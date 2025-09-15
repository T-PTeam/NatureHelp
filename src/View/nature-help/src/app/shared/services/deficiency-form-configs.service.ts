import { Validators } from "@angular/forms";
import moment from "moment";

import { EDeficiencyType } from "@/models/enums";
import { IUser } from "@/models/IUser";
import { IWaterDeficiency } from "@/modules/water-deficiency/models/IWaterDeficiency";
import { ISoilDeficiency } from "@/modules/soil-deficiency/models/ISoilDeficiency";
import { IDeficiencyFormConfig } from "@/shared/models/IDeficiencyFormConfig";

export class WaterDeficiencyFormConfig implements IDeficiencyFormConfig {
  deficiencyType = EDeficiencyType.Water;

  getSpecificFormFields(deficiency: IWaterDeficiency | null, currentUser: IUser | null): any {
    return {
      ph: [deficiency?.ph || 0, [Validators.required, Validators.min(0), Validators.max(14)]],
      dissolvedOxygen: [deficiency?.dissolvedOxygen || 0, [Validators.required, Validators.min(0), Validators.max(20)]],
      leadConcentration: [
        deficiency?.leadConcentration || 0,
        [Validators.required, Validators.min(0), Validators.max(0.01)],
      ],
      mercuryConcentration: [
        deficiency?.mercuryConcentration || 0,
        [Validators.required, Validators.min(0), Validators.max(0.001)],
      ],
      nitrateConcentration: [
        deficiency?.nitrateConcentration || 0,
        [Validators.required, Validators.min(0), Validators.max(50)],
      ],
      pesticidesContent: [
        deficiency?.pesticidesContent || 0,
        [Validators.required, Validators.min(0), Validators.max(0.005)],
      ],
      microbialActivity: [
        deficiency?.microbialActivity || 0,
        [Validators.required, Validators.min(0), Validators.max(1000)],
      ],
      radiationLevel: [deficiency?.radiationLevel || 0, [Validators.required, Validators.min(0), Validators.max(10)]],
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
        [Validators.required, Validators.min(0), Validators.max(2)],
      ],
      cadmiumConcentration: [
        deficiency?.cadmiumConcentration || 0,
        [Validators.required, Validators.min(0), Validators.max(0.005)],
      ],
      totalDissolvedSolids: [
        deficiency?.totalDissolvedSolids || 0,
        [Validators.required, Validators.min(0), Validators.max(1500)],
      ],
      electricalConductivity: [
        deficiency?.electricalConductivity || 0,
        [Validators.required, Validators.min(0), Validators.max(2500)],
      ],
      microbialLoad: [deficiency?.microbialLoad || 0, [Validators.required, Validators.min(0), Validators.max(2000)]],
    };
  }
}

export class SoilDeficiencyFormConfig implements IDeficiencyFormConfig {
  deficiencyType = EDeficiencyType.Soil;

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
