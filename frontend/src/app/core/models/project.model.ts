export interface Projects {
  id: string; // Represents the Guid
  name: string;
  projectTemplate: ProjectTemplate;
  projectKey: string;
  leadId: string; // Represents the Guid
  members: string[]; // Represents List<Guid>
  accessLevel: AccessLevel;
  createdAt: string; // Represents DateTime
  updatedAt: string; // Represents DateTime
}

export enum AccessLevel {
  Public = 0,
  Private = 1,
  Invite = 2,
}

export enum ProjectTemplate {
  Scrum = 0,
  Kanban = 1,
  BugTracker = 2,
}
