using ProjectService.API.Models;

namespace ProjectService.API.Features.CreateProject;

public record GetProjectsByLeadQuery(Guid leadId) : IRequest<GetProjectsByLeadQueryResult>;

public record GetProjectsByLeadQueryResult(List<Project> Projects);


public class GetProjectsByLeadHandler : IRequestHandler<GetProjectsByLeadQuery, GetProjectsByLeadQueryResult>
{
    public async Task<GetProjectsByLeadQueryResult> Handle(GetProjectsByLeadQuery request, CancellationToken cancellationToken)
    {
        //query db
        return new GetProjectsByLeadQueryResult(new List<Project>());
    }
}