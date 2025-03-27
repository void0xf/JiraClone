using Issues.API.Models;
using Marten;
using SharedKernel;
using SharedKernel.Utils;

namespace Issues.API.Data;

public class IssueRepository(IDocumentSession session) : IIssueRepository
{
    public async Task<Result<List<Issue>>> GetIssueByProjectId(Guid projectId, CancellationToken cancellationToken = default)
    {
        var issues = await session.Query<Issue>().Where(issue => issue.ProjectId == projectId).ToListAsync(cancellationToken);
        var issue2 =  await session.Query<Issue>().ToListAsync();
        if (!issues.Any())
        {
            return Result<List<Issue>>.Failure(
                Error.NotFound(ErrorCode.NotFound, 
                "Issue not found", 
                "Issues for this project doesn't exist"));
            
        }
        return Result<List<Issue>>.Success(issues.ToList());
    }

    public async Task<Result<Issue>> GetIssueById(Guid issueId, CancellationToken cancellationToken = default)
    {
        var issue = await session.Query<Issue>().FirstOrDefaultAsync(issue => issue.Id == issueId, cancellationToken);
        
        if (issue is null)
        {
            return Result<Issue>.Failure(
                Error.NotFound(ErrorCode.NotFound, 
                "Issue not found", 
                "The requested issue doesn't exist"));
        }
        
        return Result<Issue>.Success(issue);
    }

    public async Task<Result<Issue>> CreateIssue(Issue issue, CancellationToken cancellationToken = default)
    {
        try
        {
            session.Store(issue);
            await session.SaveChangesAsync(cancellationToken);
            return Result<Issue>.Success(issue);
        }
        catch (Exception e)
        {
            return Result<Issue>.Failure(
                Error.Conflict(ErrorCode.Conflict, e.Message, "failed to create issue"));
        }
    }

    public async Task<Result<Issue>> UpdateIssue(Guid issueId, Issue issue, CancellationToken cancellationToken = default)
    {
        var existingIssue = await session.Query<Issue>().FirstOrDefaultAsync(issue => issue.Id == issueId, cancellationToken);
        
        if (existingIssue is null)
        {
            return Result<Issue>.Failure(
                Error.NotFound(ErrorCode.NotFound,
                    "Issue not found",
                    "Issues for this project doesn't exist"));
        }
        //TODO refactor this line to handler!
        ObjectExtensions.CopyNonNullProperties(issue, existingIssue!);

        try
        {
            session.Update(existingIssue);
            await session.SaveChangesAsync(cancellationToken);
            return Result<Issue>.Success(existingIssue);
        }
        catch (Exception e)
        {
            return Result<Issue>.Failure(Error.Conflict(ErrorCode.Conflict, e.Message, "failed to update issue"));
        }
        
        
        
    }

    public async Task<Result<Issue>> DeleteIssue(Guid issueId, CancellationToken cancellationToken = default)
    {
        var existingIssue = await session.Query<Issue>().FirstOrDefaultAsync(issue => issue.Id == issueId, cancellationToken);
        
        if (existingIssue is null)
        {
            return Result<Issue>.Failure(
                Error.NotFound(ErrorCode.NotFound,
                    "Issue not found",
                    "Issues for this project doesn't exist"));
        }

        try
        {
            session.Delete(existingIssue);
            await session.SaveChangesAsync(cancellationToken);
            return Result<Issue>.Success(existingIssue);
        }
        catch (Exception e)
        {
            return Result<Issue>.Failure(Error.Conflict(ErrorCode.Conflict, e.Message, "failed to delete issue"));
        }

    }
}