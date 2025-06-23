import { EDeficiencyType } from "./enums";
import { IBaseEntity } from "./IBaseEntity";
import { IDeficiency } from "./IDeficiency";

export interface ICommentMessage extends IBaseEntity {
  message: string;
  deficiencyType: EDeficiencyType;
  deficiencyId: string;
  deficiency: IDeficiency;
  creatorFullName: string;
}
