import { EDangerState, EDeficiencyType } from "./enums";

export interface IDeficiencyMapDto {
  id: string;
  title: string;
  description: string;
  type: EDeficiencyType;
  eDangerState: EDangerState;
  longitude: number;
  latitude: number;
  radiusAffected: number;
  creatorFullName: string;
  responsibleUserFullName: string;
}
