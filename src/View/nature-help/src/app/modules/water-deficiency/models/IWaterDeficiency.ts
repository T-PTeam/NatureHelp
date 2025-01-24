import { IWaterDeficiency } from "@/models/IWaterDeficiency";

export interface IWDeficiency extends IWaterDeficiency {
    populationAffected: number;
    economicImpact: number;
    healthImpact?: string;
    resolvedDate?: Date;
    expectedResolutionDate?: Date;
    caused: string;
    waterQualityLevel: number;
  }
  
  