using ProjectService.API.Features.GetProject.DTO;
using ProjectService.API.Models;
using SharedKernel;

namespace ProjectService.API.Features.GetProject;

public class GetProjectsByLeadIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/projects", async (ISender sender) =>
        {
            var result = await sender.Send(new GetProjectsByLeadIdQuery(Guid.Empty));
            
            if (result.IsSuccess)
            {
                var resultDto = new GetProjectsByLeadIdResponse(result.Value.Projects);
                return ApiResponse<GetProjectsByLeadIdResponse>.Success(resultDto).ToMinimalApiResult();;
            }
            
            return result.ToApiResponse().ToMinimalApiResult();
        });
    }
}