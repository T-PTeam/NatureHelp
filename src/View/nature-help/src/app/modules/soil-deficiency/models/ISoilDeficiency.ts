import { IDeficiency } from "@/models/IDeficiency";

export interface ISoilDeficiency extends IDeficiency {
    ph : number;
    organicMatter: number;
    leadConcentration: number;
    cadmiumConcentration: number;
    mercuryConcentration: number;
    pesticidesContent: number;
    nitratesConcentration: number;
    heavyMetalsConcentration: number;
    electricalConductivity: number;
    toxicityLevel: number;
    microbialActivity: number;
    AnalysisDate: Date;

    populationAffected: number;
    economicImpact: number;
    healthImpact?: string;
    resolvedDate?: Date;
    expectedResolutionDate?: Date;
    caused: string;
    soilQualityLevel: number;
  }
  