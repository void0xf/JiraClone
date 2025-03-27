using Issues.API.Data;
using Issues.API.Models;
using Marten;
using MediatR;
using SharedKernel;

namespace Issues.API.Features.CreateIssue;

public record CreateIssueCommand(
    string Key,
    Guid ProjectId,
    IssueType IssueType,
    string? ParentIssueId,
    string Summary,
    string Description,
    IssueStatus Status,
    IssuePriority Priority,
    string? AssigneeId,
    string ReporterId,
    string? SprintId,
    List<Label>? Labels,
    int? EstimatedStoryPoints,
    bool IsArchived,
    bool InBacklog,
    DateTime? DueDate
) : IRequest<Result<CreateIssueResult>>;

public record CreateIssueResult(Guid IssueId, string Key);

public class CreateIssueHandler(IIssueRepository issueRepository) : IRequestHandler<CreateIssueCommand, Result<CreateIssueResult>>
{
    public async Task<Result<CreateIssueResult>> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
    {
        var issue = new Issue
        {
            Id = Guid.NewGuid(),
            Key = request.Key,
            ProjectId = request.ProjectId,
            IssueType = request.IssueType,
            ParentIssueId = request.ParentIssueId,
            Summary = request.Summary,
            Description = request.Description,
            Status = request.Status,
            Priority = request.Priority,
            AssigneeId = request.AssigneeId,
            ReporterId = request.ReporterId,
            SprintId = request.SprintId,
            Labels = request.Labels ?? new List<Label>(),
            EstimatedStoryPoints = request.EstimatedStoryPoints,
            IsArchived = request.IsArchived,
            InBacklog = request.InBacklog,
            DueDate = request.DueDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Activity = new IssueActivity 
            { 
                Id = Guid.NewGuid(),
                Comments = new List<Comment>(),
                Attachments = new List<Attachment>(),
                History = new List<IssueHistory>(),
                Worklogs = new List<Worklog>()
            }
        };
        
        var result = await issueRepository.CreateIssue(issue, cancellationToken);
        
        if (result.IsFailure)
        {
            return Result<CreateIssueResult>.Failure(result.Error);
        }
        
        return Result<CreateIssueResult>.Success(new CreateIssueResult(result.Value.Id, result.Value.Key));
    }
}