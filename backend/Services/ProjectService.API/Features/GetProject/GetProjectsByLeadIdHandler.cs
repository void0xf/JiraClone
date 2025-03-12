using ProjectService.API.Models;

namespace ProjectService.API.Features.GetProject;

public record GetProjectsByLeadIdQuery() : IRequest<GetProjectsByLeadIdQueryResult>;

public record GetProjectsByLeadIdQueryResult(List<Project> Projects);


public class GetProjectsByLeadIdHandler(IDocumentSession session) : IRequestHandler<GetProjectsByLeadIdQuery, GetProjectsByLeadIdQueryResult>
{
    public async Task<GetProjectsByLeadIdQueryResult> Handle(GetProjectsByLeadIdQuery request, CancellationToken cancellationToken)
    {
        var projects = await session.Query<Project>()
            //.Where(p => p.LeadId == request.LeadId) auth needed for that
            .ToListAsync(cancellationToken);   
        return new GetProjectsByLeadIdQueryResult(projects.ToList());
    }
}