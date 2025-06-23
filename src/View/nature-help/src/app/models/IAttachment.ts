import { IBaseEntity } from "./IBaseEntity";

export interface IAttachment extends IBaseEntity {
  fileName: string;
  fileSize: number;
  mimeType: string;
  storagePath: string;
  previewUrl: string;
}

export interface IDeficiencyAttachment extends IAttachment {
  deficiencyId: string;
  deficiency: any;
}
