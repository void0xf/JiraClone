using System.Text.Json.Serialization;
using Issues.API.Models;

namespace Issues.API.Features.GetIssueById.DTO;

public record GetIssueByIdResponse(
    Guid Id,
    string Key,
    Guid ProjectId,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    IssueType IssueType,
    string? ParentIssueId,
    string Summary,
    string Description,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    IssueStatus Status,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    IssuePriority Priority,
    string? AssigneeId,
    string ReporterId,
    string? SprintId,
    List<LabelDto> Labels,
    int? EstimatedStoryPoints,
    bool IsArchived,
    bool InBacklog,
    DateTime? DueDate,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record LabelDto(Guid Id, string Name); 