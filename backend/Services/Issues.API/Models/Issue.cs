namespace Issues.API.Models;

public class Issue
{
    public Guid Id { get; set; } = default!;  // Unique identifier
    public string Key { get; set; } = default!;  // Project-prefixed ID (e.g., "PROJ-123")
    public Guid ProjectId { get; set; } = default!;  // Reference to parent project
    public IssueType IssueType { get; set; }  // Type of issue (Bug, Story, Task, Epic)
    public string? ParentIssueId { get; set; }  // Optional parent issue reference (for subtasks)
    public string Summary { get; set; } = default!;  // Brief description
    public string Description { get; set; } = default!;  // Full markdown description
    public IssueStatus Status { get; set; }  // Current status (ToDo, In Progress, Done)
    public IssuePriority Priority { get; set; }  // Importance level
    public string? AssigneeId { get; set; }  // User assigned to the issue
    public string ReporterId { get; set; } = default!;  // User who created the issue
    public string? SprintId { get; set; }  // Current sprint reference
    public List<Label> Labels { get; set; } = new List<Label>();  // Labels associated with the issue
    public int? EstimatedStoryPoints { get; set; }  // Agile estimation points
    public bool IsArchived { get; set; }  // Whether the issue is archived
    public bool InBacklog { get; set; }  // Whether it is in the backlog
    public DateTime? DueDate { get; set; }  // Deadline if applicable
    public DateTime CreatedAt { get; set; }  // Creation timestamp
    public DateTime UpdatedAt { get; set; }  // Last update timestamp
    public IssueActivity Activity { get; set; } = new IssueActivity();  // Encapsulated activity logs
}

// Enums for issue-related fields
public enum IssueType { Bug, Story, Task, Epic, Subtask }
public enum IssueStatus { ToDo, InProgress, Done, Blocked, InReview }
public enum IssuePriority { Lowest, Low, Medium, High, Highest }

// Label Model
public class Label
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
}

// **Encapsulated IssueActivity Class**
public class IssueActivity
{
    public Guid Id { get; set; } = default!;
    public List<Comment> Comments { get; set; } = new List<Comment>();  // Comments on the issue
    public List<Attachment> Attachments { get; set; } = new List<Attachment>();  // Attached files
    public List<IssueHistory> History { get; set; } = new List<IssueHistory>();  // Change history
    public List<Worklog> Worklogs { get; set; } = new List<Worklog>();  // Time tracking records
}

// Comments Model
public class Comment
{
    public Guid Id { get; set; } = default!;
    public string AuthorId { get; set; } = default!;
    public string Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}

// Attachments Model
public class Attachment
{
    public Guid Id { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string FileUrl { get; set; } = default!;
    public string UploadedById { get; set; } = default!;
    public DateTime UploadedAt { get; set; }
}

// Subtasks Model (to support hierarchical issues)
public class Subtask
{
    public Guid Id { get; set; } = default!;
    public string Summary { get; set; } = default!;
    public IssueStatus Status { get; set; }
}

// Issue History Model (Tracks changes)
public class IssueHistory
{
    public Guid Id { get; set; } = default!;
    public string ChangedById { get; set; } = default!;
    public string Field { get; set; } = default!;
    public string OldValue { get; set; } = default!;
    public string NewValue { get; set; } = default!;
    public DateTime ChangedAt { get; set; }
}

// Worklog Model (Time tracking)
public class Worklog
{
    public Guid Id { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public int TimeSpentMinutes { get; set; }
    public string Description { get; set; } = default!;
    public DateTime LoggedAt { get; set; }
}
