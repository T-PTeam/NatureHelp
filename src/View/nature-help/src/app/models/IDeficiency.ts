import { EDangerState, EDeficiencyType } from "./enums";
import { IBaseEntity } from "./IBaseEntity";
import { IChangedModelLog } from "./IChangedModelLog";
import { IUser } from "./IUser";

export interface IDeficiency extends IBaseEntity {
  title: string;
  description: string;
  type: EDeficiencyType;
  creator: IUser;
  responsibleUser?: IUser;
  latitude: number;
  longitude: number;
  radiusAffected: number;
  eDangerState: EDangerState;
  changedModelLogId: string;
  changedModelLog: IChangedModelLog[];
}
