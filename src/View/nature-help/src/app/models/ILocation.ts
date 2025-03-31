import { ICoordinates } from "./ICoordinates";

export interface ILocation extends ICoordinates {
  country: string;
  region: string;
  district: string;
  city: string;
  radiusAffected: number;
}
