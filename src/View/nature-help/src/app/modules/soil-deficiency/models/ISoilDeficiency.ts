import { ISoilDeficiency } from "@/models/ISoilDeficiency";

export interface IDeficiency extends ISoilDeficiency {
    populationAffected: number;
    economicImpact: number;
    healthImpact?: string;
    resolvedDate?: Date;
    expectedResolutionDate?: Date;
    caused: string;
    soilQualityLevel: number;
  }
  