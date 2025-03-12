using ProjectService.API.Models;
using SharedKernel;

namespace ProjectService.API.Features.GetProject;

public record GetProjectsByLeadIdQueryResponse(List<Project> Projects);


public class GetProjectsByLeadIdEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/projects", async (ISender sender) =>
        {
            var result = await sender.Send(new GetProjectsByLeadIdQuery(Guid.Empty));
            return result.ToMinimalApiResult();
        });
    }
}