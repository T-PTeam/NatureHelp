import { IDeficiency } from "@/models/IDeficiency";

export interface ISoilDeficiency extends IDeficiency {
  ph: number;
  organicMatter: number;
  leadConcentration: number;
  cadmiumConcentration: number;
  mercuryConcentration: number;
  pesticidesContent: number;
  nitrateConcentration: number;
  heavyMetalsConcentration: number;
  electricalConductivity: number;
  microbialActivity: number;
  analysisDate: Date;
}
