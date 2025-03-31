import { IDeficiency } from "@/models/IDeficiency";

export interface IWaterDeficiency extends IDeficiency {
  ph: number;
  dissolvedOxygen: number;
  leadConcentration: number;
  mercuryConcentration: number;
  nitratesConcentration: number;
  pesticidesContent: number;
  microbialActivity: number;
  radiationLevel: number;
  chemicalOxygenDemand: number;
  biologicalOxygenDemand: number;
  phosphateConcentration: number;
  cadmiumConcentration: number;
  totalDissolvedSolids: number;
  electricalConductivity: number;
  microbialLoad: number;
}
