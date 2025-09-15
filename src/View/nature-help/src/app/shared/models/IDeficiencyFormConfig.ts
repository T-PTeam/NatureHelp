import { EDeficiencyType } from "@/models/enums";
import { IDeficiencyAttachment } from "@/models/IAttachment";
import { IDeficiency } from "@/models/IDeficiency";
import { IUser } from "@/models/IUser";
import { FormGroup } from "@angular/forms";
import { IAddress } from "../services/map-view.service";

export interface IDeficiencyDetailsState {
  details: IDeficiency | null;
  detailsForm: FormGroup;
  dangerStates: any[];
  isSelectingCoordinates: boolean;
  selectedAddress: IAddress | null;
  attachments: IDeficiencyAttachment[];
  isUploading: boolean;
  uploadProgress: number;
  lastUploadError: string | null;
  lastUploadedFiles: File[];
  isAddingDeficiency: boolean;
  currentUser: IUser | null;
}

export interface IDeficiencyFormConfig {
  deficiencyType: EDeficiencyType;
  getSpecificFormFields: (deficiency: any, currentUser: IUser | null) => any;
}
