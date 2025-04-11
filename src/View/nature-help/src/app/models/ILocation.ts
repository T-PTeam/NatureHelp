import { ICoordinates } from "./ICoordinates";

export interface ILocation extends ICoordinates {
  id: string;
  country: string;
  region: string;
  district: string;
  city: string;
  radiusAffected: number;
}
