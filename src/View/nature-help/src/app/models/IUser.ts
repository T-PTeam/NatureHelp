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
  isEmailConfirmed?: boolean;
  deficiencyMonitoringScheme?: IMonitoringScheme;
  profileIsPublic?: boolean;
  emailNotificationsEnabled?: boolean;
  achievementAlertsEnabled?: boolean;
  newsletterEnabled?: boolean;
}
