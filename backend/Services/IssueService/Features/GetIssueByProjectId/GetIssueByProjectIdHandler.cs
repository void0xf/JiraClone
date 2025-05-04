using Issues.API.Data;
using Issues.API.Models;
using MediatR;
using SharedKernel;

namespace Issues.API.Features.GetIssueByProjectId;

public record GetIssueByProjectIdQuery(Guid projectId) : IRequest<Result<GetIssueByProjectIdResult>>;
public record GetIssueByProjectIdResult(List<Issue> Issues);
    
public class GetIssueByProjectIdHandler(IIssueRepository repository) : IRequestHandler<GetIssueByProjectIdQuery, Result<GetIssueByProjectIdResult>>
{
    public async Task<Result<GetIssueByProjectIdResult>> Handle(GetIssueByProjectIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetIssueByProjectId(request.projectId, cancellationToken);
        if (result.IsFailure)
        {
            return Result<GetIssueByProjectIdResult>.Failure(result.Error);
        }
        return Result<GetIssueByProjectIdResult>.Success(new GetIssueByProjectIdResult(result.Value));
    }
}