import { EDeficiencyType } from "./enums";
import { IBaseEntity } from "./IBaseEntity";

export interface IChangedModelLog extends IBaseEntity {
  deficiencyType: EDeficiencyType;
  deficiencyId: string;
  changesJson: string;
}
