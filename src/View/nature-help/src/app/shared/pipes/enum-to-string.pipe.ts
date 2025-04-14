import { Pipe, PipeTransform } from "@angular/core";

import { EDangerState, EResearchType } from "@/models/enums";

@Pipe({
  name: "enumToString",
  standalone: false,
})
export class EnumToStringPipe implements PipeTransform {
  transform(value: any, type: string): string {
    if (type === "EDangerState") {
      return EDangerState[value] ?? value;
    } else if (type === "ResearchType") {
      const formattedValue = EResearchType[value] ?? value;
      return formattedValue.replace(/([A-Z])/g, " $1").trim();
    }

    return value;
  }
}
