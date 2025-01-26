import { IDeficiency } from "@/models/IDeficiency";

export interface ISoilDeficiency extends IDeficiency {
    populationAffected: number;
    economicImpact: number;
    healthImpact?: string;
    resolvedDate?: Date;
    expectedResolutionDate?: Date;
    caused: string;
    soilQualityLevel: number;
  }
  