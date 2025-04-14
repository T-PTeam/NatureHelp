import { EDangerState } from "@/models/enums";

export interface ISoilDeficiencyFilter {
  title: string;
  description: string;
  eDangerState: EDangerState;
}
