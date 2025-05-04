using System.Text.RegularExpressions;

namespace ProjectService.API.Models;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ProjectTemplate ProjectTemplate { get; set; }
    public string ProjectKey { get; set; } = string.Empty;
    public Guid LeadId { get; set; }
    public List<Guid> Members { get; set; } = new();
    public AccessLevel AccessLevel { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public enum AccessLevel
{
    Public,
    Private,
    Invite
}

public enum ProjectTemplate
{
    Scrum,
    Kanban,
    BugTracker
}