using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Issues.API.Models;

namespace Issues.API.Features.CreateIssue.DTO
{
    public class CreateIssueRequest
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("projectId")]
        public Guid ProjectId { get; set; }

        [JsonPropertyName("issueType")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public IssueType IssueType { get; set; }

        [JsonPropertyName("parentIssueId")]
        public string? ParentIssueId { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("status")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public IssueStatus Status { get; set; }

        [JsonPropertyName("priority")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public IssuePriority Priority { get; set; }

        [JsonPropertyName("assigneeId")]
        public string? AssigneeId { get; set; }

        [JsonPropertyName("reporterId")]
        public string ReporterId { get; set; }

        [JsonPropertyName("sprintId")]
        public string? SprintId { get; set; }

        [JsonPropertyName("labels")]
        public List<LabelDto>? Labels { get; set; }

        [JsonPropertyName("estimatedStoryPoints")]
        public int? EstimatedStoryPoints { get; set; }

        [JsonPropertyName("isArchived")]
        public bool IsArchived { get; set; }

        [JsonPropertyName("inBacklog")]
        public bool InBacklog { get; set; }

        [JsonPropertyName("dueDate")]
        public DateTime? DueDate { get; set; }
    }

    public class LabelDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
