import { Comment } from './comment.model';
import { Attachment } from './attachment.model';
import { IssueHistory } from './issue-history.model';
import { Worklog } from './worklog.model';

/**
 * Encapsulated IssueActivity Class
 */
export interface IssueActivity {
  id: string;
  comments: Comment[];
  attachments: Attachment[];
  history: IssueHistory[];
  worklogs: Worklog[];
} 