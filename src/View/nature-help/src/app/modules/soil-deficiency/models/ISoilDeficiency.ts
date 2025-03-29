import { IDeficiency } from "@/models/IDeficiency";

export interface ISoilDeficiency extends IDeficiency {
  ph: number;
  organicMatter: number;
  leadConcentration: number;
  cadmiumConcentration: number;
  mercuryConcentration: number;
  pesticidesContent: number;
  nitratesConcentration: number;
  heavyMetalsConcentration: number;
  electricalConductivity: number;
  toxicityLevel: string;
  microbialActivity: number;
  analysisDate: Date;
}
