export interface IUser {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  passwordHash: string;
  role: number;
  organizationId: string | null;
}
