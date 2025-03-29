export interface IUser {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  role: number;
  organizationId: string | null;
}
