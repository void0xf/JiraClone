using Issues.API.Data;
using Issues.API.Models;
using Marten;
using MediatR;
using SharedKernel;

namespace Issues.API.Features.GetIssueById;

public record GetIssueByIdQuery(Guid IssueId) : IRequest<Result<GetIssueByIdResult>>;
public record GetIssueByIdResult(Issue Issue);

public class GetIssueByIdHandler(IDocumentSession session) : IRequestHandler<GetIssueByIdQuery, Result<GetIssueByIdResult>>
{
    public async Task<Result<GetIssueByIdResult>> Handle(GetIssueByIdQuery request, CancellationToken cancellationToken)
    {
        var issue = await session.Query<Issue>().FirstOrDefaultAsync(i => i.Id == request.IssueId, cancellationToken);

        if (issue == null)
        {
            return Result<GetIssueByIdResult>.Failure(
                Error.NotFound(ErrorCode.NotFound, 
                "Issue not found", 
                "The requested issue doesn't exist"));
        }

        return Result<GetIssueByIdResult>.Success(new GetIssueByIdResult(issue));
    }
} 