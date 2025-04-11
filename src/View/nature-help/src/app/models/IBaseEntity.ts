import { IUser } from "./IUser";

export interface IBaseEntity {
  id: string;
  createdBy: IUser;
  createdOn: Date;
}
