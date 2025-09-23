import { IBaseEntity } from "./IBaseEntity";

export interface IOrganization extends IBaseEntity {
  title: string;
  allowedMembersCount: number;
}
