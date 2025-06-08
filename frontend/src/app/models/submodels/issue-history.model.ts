/**
 * Issue History Model (Tracks changes)
 */
export interface IssueHistory {
  id: string;
  changedById: string;
  field: string;
  oldValue: string;
  newValue: string;
  changedAt: string;
} 