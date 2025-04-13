import { EDangerState } from "@/models/enums";

export interface IWaterDeficiencyFilter {
  title: string;
  description: string;
  eDangerState: EDangerState;
}
