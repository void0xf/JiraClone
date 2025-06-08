/**
 * Worklog Model (Time tracking)
 */
export interface Worklog {
  id: string;
  userId: string;
  timeSpentMinutes: number;
  description: string;
  loggedAt: string;
} 