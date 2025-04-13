import { EResearchType } from "@/models/enums";
import { IUser } from "@/models/IUser";
import { ILaboratory } from "./ILaboratory";

export interface IResearch {
  id: string;
  title: string;
  type: EResearchType;
  date: Date;
  description: string;
  laboratory: ILaboratory;
  researcher: IUser;
}
