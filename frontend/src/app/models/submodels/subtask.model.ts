import { IssueStatus } from '../issue.model';

/**
 * Subtasks Model
 */
export interface Subtask {
  id: string;
  summary: string;
  status: IssueStatus;
} 