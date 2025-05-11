using System.Security.Claims;
using ProjectService.API.Models;
using SharedKernel;

namespace ProjectService.API.Features.GetProject;

public record GetProjectsByLeadIdQuery() : IRequest<Result<GetProjectsByLeadIdQueryResult>>;

public record GetProjectsByLeadIdQueryResult(IReadOnlyList<Project> Projects);


public class GetProjectsByLeadIdHandler(IDocumentSession session,IHttpContextAccessor _httpContextAccessor) : IRequestHandler<GetProjectsByLeadIdQuery, Result<GetProjectsByLeadIdQueryResult>>
{
    public async Task<Result<GetProjectsByLeadIdQueryResult>> Handle(GetProjectsByLeadIdQuery request, 
        CancellationToken cancellationToken)
    {
        var principal = _httpContextAccessor.HttpContext?.User;
        var keycloakUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        
        var projects = await session.Query<Project>()
            .Where(p => p.LeadId == Guid.Parse(keycloakUserId))
            .ToListAsync(cancellationToken);
        if(!projects.Any()) return Result<GetProjectsByLeadIdQueryResult>
            .Failure(Error.NotFound(
                ErrorCode.ProjectsFailedToLoad,  
                "No projects found for the specified lead.", 
                "No projects found."));
        
        return Result<GetProjectsByLeadIdQueryResult>.Success(new GetProjectsByLeadIdQueryResult(projects));
    }
}