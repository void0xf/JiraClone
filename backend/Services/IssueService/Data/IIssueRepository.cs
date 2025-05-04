using Issues.API.Models;
using SharedKernel;

namespace Issues.API.Data;

public interface IIssueRepository
{
    public Task<Result<List<Issue>>> GetIssueByProjectId(Guid projectId, CancellationToken cancellationToken);
    public Task<Result<Issue>> GetIssueById(Guid issueId, CancellationToken cancellationToken);
    public Task<Result<Issue>> CreateIssue(Issue issue, CancellationToken cancellationToken);
    public Task<Result<Issue>> UpdateIssue(Guid issueId, Issue issue, CancellationToken cancellationToken);
    public Task<Result<Issue>> DeleteIssue(Guid issueId, CancellationToken cancellationToken);
    
}