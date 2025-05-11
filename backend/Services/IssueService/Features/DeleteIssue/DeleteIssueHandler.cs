using Issues.API.Data;
using MediatR;
using SharedKernel;

namespace Issues.API.Features.DeleteIssue;

public record DeleteIssueCommand(Guid IssueId) : IRequest<Result<DeleteIssueResult>>;

public record DeleteIssueResult(Guid IssueId);

public class DeleteIssueHandler(IIssueRepository issueRepository) : IRequestHandler<DeleteIssueCommand, Result<DeleteIssueResult>>
{
    public async Task<Result<DeleteIssueResult>> Handle(DeleteIssueCommand request, CancellationToken cancellationToken)
    {
        var issue = await issueRepository.GetIssueById(request.IssueId, cancellationToken);
        if (issue == null)
            return Result<DeleteIssueResult>.Failure(Error.NotFound(ErrorCode.NotFound, "Issue Not Found","Issue Not Found" ));
        
        var result = await issueRepository.DeleteIssue(request.IssueId, cancellationToken);
        
        if (result.IsFailure)
        {
            return Result<DeleteIssueResult>.Failure(result.Error);
        }
        
        return Result<DeleteIssueResult>.Success(new DeleteIssueResult(result.Value.Id));
    }
} 