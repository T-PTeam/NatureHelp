import { ILocation } from "@/models/ILocation";
import { IUser } from "@/models/IUser";

export interface ILaboratory {
    id: string,
    title: string,
    researches: IUser[],
    location: ILocation,
    researchersCount: Number,
}
