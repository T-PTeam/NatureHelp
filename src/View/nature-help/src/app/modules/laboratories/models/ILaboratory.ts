import { IUser } from "@/models/IUser";

export interface ILaboratory {
  id: string;
  title: string;
  researchers: IUser[];
  latitude: number;
  longitude: number;
  researchersCount: number;
}
