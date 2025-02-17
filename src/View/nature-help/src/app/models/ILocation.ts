import { ICoordinates } from "./ICoordinates";

export interface ILocation extends ICoordinates {
    city: string;
    country: string;
  }