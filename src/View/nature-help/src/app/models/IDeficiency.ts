import { EDeficiencyType, EDangerState } from "./enums";
import { ILocation } from "@/modules/soil-deficiency/models/ILocation";
import { IBaseEntity } from "./IBaseEntity";
import { IUser } from "./IUser";

export interface IDeficiency extends IBaseEntity {
    title: string;
    description: string;
    type: EDeficiencyType;
    creator: IUser;
    responsibleUser?: IUser;
    location: ILocation;
    eDangerState: EDangerState;
  }

  