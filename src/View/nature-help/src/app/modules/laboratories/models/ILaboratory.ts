import { ILocation } from "@/models/ILocation";
import { IUser } from "@/models/IUser";

export interface ILaboratory {
    id: number,
    title: string,
    researches: IUser,
    location: ILocation,
}
