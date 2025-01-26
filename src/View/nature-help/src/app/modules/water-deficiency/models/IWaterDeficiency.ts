import { IDeficiency } from "@/models/IDeficiency";

export interface IWaterDeficiency extends IDeficiency {
    ph : number;
    dissolvedOxygen: number;
    leadConcentration: number;
    populationAffected: number;
    mercuryConcentration: number;
    nitrateConcentration: number;
    pesticidesContent: number;
    microbialActivity: number;
    radiationLevel: number;
    chemicalOxygenDemand: number;
    biologicalOxygenDemand: number;
    toxicityLevel: number;
    phosphateConcentration: number;
    cadmiumConcentration: number;
    totalDissolvedSolids: number;
    electricalConductivity: number;
    microbialLoad: number;

    economicImpact: number;
    healthImpact?: string;
    resolvedDate?: Date;
    expectedResolutionDate?: Date;
    caused: string;
    waterQualityLevel: number;
  }
  
  