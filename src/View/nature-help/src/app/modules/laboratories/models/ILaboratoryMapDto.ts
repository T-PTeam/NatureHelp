import { IResearcherDto } from "./IResearcherDto";

export interface ILaboratoryMapDto {
  id: string;
  title: string;
  longitude: number;
  latitude: number;
  researchersCount: number;
  researchers: IResearcherDto[];
}
