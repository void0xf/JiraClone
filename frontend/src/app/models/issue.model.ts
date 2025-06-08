import { Label } from './submodels/label.model';
import { IssueActivity } from './submodels/issue-activity.model';

export enum IssueType {
  Bug = 'Bug',
  Story = 'Story',
  Task = 'Task',
  Epic = 'Epic',
  Subtask = 'Subtask', 
}

export enum IssueStatus {
  ToDo = 'ToDo',
  InProgress = 'InProgress',
  Done = 'Done',
  Blocked = 'Blocked',
  InReview = 'InReview',
}

export enum IssuePriority {
  Lowest = 'Lowest',
  Low = 'Low',
  Medium = 'Medium',
  High = 'High',
  Highest = 'Highest',
}

export interface Issue {
  id: string;
  key: string;
  projectId: string;
  issueType: IssueType;
  parentIssueId: string | null;
  summary: string;
  description: string;
  status: IssueStatus;
  priority: IssuePriority;
  assigneeId: string | null;
  reporterId: string;
  sprintId: string | null;
  labels: Label[];
  estimatedStoryPoints: number | null;
  isArchived: boolean;
  inBacklog: boolean;
  dueDate: string | null;
  createdAt: string;
  updatedAt: string;
  activity: IssueActivity;

} 