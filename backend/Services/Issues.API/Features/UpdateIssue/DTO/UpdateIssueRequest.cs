using System.Text.Json.Serialization;
using Issues.API.Models;

namespace Issues.API.Features.UpdateIssue.DTO;

public record UpdateIssueRequest(
    string? Key,
    Guid? ProjectId,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    IssueType? IssueType,
    string? ParentIssueId,
    string? Summary,
    string? Description,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    IssueStatus? Status,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    IssuePriority? Priority,
    string? AssigneeId,
    string? ReporterId,
    string? SprintId,
    List<LabelDto>? Labels,
    int? EstimatedStoryPoints,
    bool? IsArchived,
    bool? InBacklog,
    DateTime? DueDate);

public record LabelDto(string Name); 