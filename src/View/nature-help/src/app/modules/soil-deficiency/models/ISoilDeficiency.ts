import { ISoilDeficiency } from "@/models/ISoilDeficiency";

export interface ISDeficiency extends ISoilDeficiency {
    populationAffected: number;
    economicImpact: number;
    healthImpact?: string;
    resolvedDate?: Date;
    expectedResolutionDate?: Date;
    caused: string;
    soilQualityLevel: number;
  }
  