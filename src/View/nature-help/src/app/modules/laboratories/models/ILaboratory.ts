import { IBaseEntity } from "@/models/IBaseEntity";
import { IUser } from "@/models/IUser";

export interface ILaboratory extends IBaseEntity {
  id: string;
  title: string;
  researchers: IUser[];
  latitude: number;
  longitude: number;
  researchersCount: number;
  address?: string;
  isPublic: boolean;
}
