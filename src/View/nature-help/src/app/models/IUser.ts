import { ILaboratoryReference } from "./ILaboratoryReference";
import { IMonitoringScheme } from "./IMonitoringScheme";

export interface IUser {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  passwordHash: string;
  role: number;
  organizationId: string | null;
  laboratoryId?: string | null;
  laboratories?: ILaboratoryReference[];
  createdOn?: Date;
  isEmailConfirmed?: boolean;
  deficiencyMonitoringScheme?: IMonitoringScheme;
  profileIsPublic?: boolean;
  emailNotificationsEnabled?: boolean;
  achievementAlertsEnabled?: boolean;
  newsletterEnabled?: boolean;
}
