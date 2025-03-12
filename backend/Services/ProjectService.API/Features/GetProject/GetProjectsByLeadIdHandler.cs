using ProjectService.API.Models;
using SharedKernel;

namespace ProjectService.API.Features.GetProject;

public record GetProjectsByLeadIdQuery(Guid LeadId) : IRequest<Result<GetProjectsByLeadIdQueryResult>>;

public record GetProjectsByLeadIdQueryResult(IReadOnlyList<Project> Projects);


public class GetProjectsByLeadIdHandler(IDocumentSession session) : IRequestHandler<GetProjectsByLeadIdQuery, Result<GetProjectsByLeadIdQueryResult>>
{
    public async Task<Result<GetProjectsByLeadIdQueryResult>> Handle(GetProjectsByLeadIdQuery request, 
        CancellationToken cancellationToken)
    {
        var projects = await session.Query<Project>()
            .Where(p => p.LeadId == request.LeadId)
            .ToListAsync(cancellationToken);
        if(!projects.Any()) return Result<GetProjectsByLeadIdQueryResult>
            .Failure(Error.NotFound(
                ErrorCode.ProjectsFailedToLoad,  
                "No projects found for the specified lead.", 
                "No projects found."));
        
        return Result<GetProjectsByLeadIdQueryResult>.Success(new GetProjectsByLeadIdQueryResult(projects));
    }
}