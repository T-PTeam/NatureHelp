export interface IAchievement {
  id: string;
  key: string;
  title: string;
  description: string;
  icon: string;
  unlockedAt: string | null;
  completed?: boolean;
  progress?: number;
  target?: number;
  kind?: number;
}
