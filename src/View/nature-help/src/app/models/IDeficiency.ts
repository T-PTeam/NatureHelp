import { EDeficiencyType, EDangerState } from "@/modules/water-deficiency/models/enums";
import { ILocation } from "@/modules/water-deficiency/models/ILocation";
import { IMetric } from "@/modules/water-deficiency/models/IMetric";
import { IBaseEntity } from "./IBaseEntity";
import { IUser } from "./IUser";

export interface IDeficiency extends IBaseEntity {
    title: string;
    description: string;
    type: EDeficiencyType;
    creator: IUser;
    responsibleUser?: IUser;
    metrics?: IMetric[];
    location: ILocation;
    eDangerState: EDangerState;
  }