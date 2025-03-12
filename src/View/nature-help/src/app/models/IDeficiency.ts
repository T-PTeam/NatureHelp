import { EDangerState, EDeficiencyType } from "./enums";
import { IBaseEntity } from "./IBaseEntity";
import { ILocation } from "./ILocation";
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
