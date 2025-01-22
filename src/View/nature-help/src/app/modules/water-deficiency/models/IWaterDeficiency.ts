import { IDeficiency } from "@/models/IDeficiency";

export interface IWaterDeficiency extends IDeficiency {
    populationAffected: number;
    economicImpact: number;
    healthImpact?: string;
    resolvedDate?: Date;
    expectedResolutionDate?: Date;
    caused: string;
    waterQualityLevel: number;
  }
  
  