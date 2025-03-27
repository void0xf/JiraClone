using Issues.API.Data;
using Issues.API.Models;
using MediatR;
using SharedKernel;
using SharedKernel.Utils;
using System.Text.Json;

namespace Issues.API.Features.UpdateIssue;

public record UpdateIssueCommand(
    Guid Id,
    string? Key,
    Guid? ProjectId,
    IssueType? IssueType,
    string? ParentIssueId,
    string? Summary,
    string? Description,
    IssueStatus? Status,
    IssuePriority? Priority,
    string? AssigneeId,
    string? ReporterId,
    string? SprintId,
    List<Label>? Labels,
    int? EstimatedStoryPoints,
    bool? IsArchived,
    bool? InBacklog,
    DateTime? DueDate,
    HashSet<string> ExplicitlySetProperties
) : IRequest<Result<UpdateIssueResult>>;

public record UpdateIssueResult(Guid IssueId);

public class UpdateIssueHandler(IIssueRepository issueRepository) : IRequestHandler<UpdateIssueCommand, Result<UpdateIssueResult>>
{
    public async Task<Result<UpdateIssueResult>> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
    {
        // First, fetch the existing issue to ensure it exists
        var existingIssueResult = await issueRepository.GetIssueById(request.Id, cancellationToken);
        
        if (existingIssueResult.IsFailure)
        {
            return Result<UpdateIssueResult>.Failure(existingIssueResult.Error);
        }
        
        var existingIssue = existingIssueResult.Value;
        
        // Create a partial issue with the properties to update
        var partialIssue = new Issue
        {
            Key = request.Key,
            ProjectId = request.ProjectId ?? default,
            IssueType = request.IssueType ?? default,
            ParentIssueId = request.ParentIssueId,
            Summary = request.Summary,
            Description = request.Description,
            Status = request.Status ?? default,
            Priority = request.Priority ?? default,
            AssigneeId = request.AssigneeId,
            ReporterId = request.ReporterId,
            SprintId = request.SprintId,
            Labels = request.Labels,
            EstimatedStoryPoints = request.EstimatedStoryPoints,
            IsArchived = request.IsArchived ?? default,
            InBacklog = request.InBacklog ?? default,
            DueDate = request.DueDate,
            // Always update timestamp
            UpdatedAt = DateTime.UtcNow
        };
        
        // Create an issue with only the updated parts
        var updateIssue = new Issue
        {
            Id = request.Id,
            // Copy current values as defaults
            Key = existingIssue.Key,
            ProjectId = existingIssue.ProjectId,
            IssueType = existingIssue.IssueType,
            ParentIssueId = existingIssue.ParentIssueId,
            Summary = existingIssue.Summary,
            Description = existingIssue.Description,
            Status = existingIssue.Status,
            Priority = existingIssue.Priority,
            AssigneeId = existingIssue.AssigneeId,
            ReporterId = existingIssue.ReporterId,
            SprintId = existingIssue.SprintId,
            Labels = existingIssue.Labels,
            EstimatedStoryPoints = existingIssue.EstimatedStoryPoints,
            IsArchived = existingIssue.IsArchived,
            InBacklog = existingIssue.InBacklog,
            DueDate = existingIssue.DueDate,
            CreatedAt = existingIssue.CreatedAt,
            UpdatedAt = DateTime.UtcNow // This will always be updated
        };
        
        // Update only properties that were explicitly set in the request
        ObjectExtensions.CopyPropertiesForPatch(partialIssue, updateIssue, request.ExplicitlySetProperties);
                
        var result = await issueRepository.UpdateIssue(request.Id, updateIssue, cancellationToken);
        
        if (result.IsFailure)
        {
            return Result<UpdateIssueResult>.Failure(result.Error);
        }
        
        return Result<UpdateIssueResult>.Success(new UpdateIssueResult(result.Value.Id));
    }
} 