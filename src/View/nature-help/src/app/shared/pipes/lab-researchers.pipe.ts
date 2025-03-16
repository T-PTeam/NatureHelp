import { Pipe, PipeTransform } from "@angular/core";
import { IUser } from "@/models/IUser";

@Pipe({
    name: "labResearchers",
    standalone: false,
})
export class LabResearchersPipe implements PipeTransform {
    transform(researchers: IUser[]): string {
        if (researchers.length > 5){
            return researchers.slice(0, 5).map(r => `${r.firstName} ${r.lastName}`).join(", ") + `and other ${researchers.length - 5} researchers`
        }

        return researchers.map(r => `${r.firstName} ${r.lastName}`).join(", ");
    }
}
