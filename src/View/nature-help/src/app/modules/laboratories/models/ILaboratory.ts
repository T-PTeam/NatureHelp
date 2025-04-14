import { ILocation } from "@/models/ILocation";
import { IUser } from "@/models/IUser";

export interface ILaboratory {
  id: string;
  title: string;
  researchers: IUser[];
  location: ILocation;
  researchersCount: number;
}
