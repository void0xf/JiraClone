using JasperFx.CodeGeneration;
using SharedKernel;
using ProjectService.API.Features.RemoveProject.DTO;

namespace ProjectService.API.Features.RemoveProject;

public class RemoveProjectByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("projects/{id}", async (ISender sender, Guid id) =>
        {
            var result = await sender.Send(new RemoveProjectCommand(id));
            
            if (result.IsSuccess)
            {
                var resultDto = result.Value.ProjectId.Adapt<RemoveProjectResponse>();
                return ApiResponse<RemoveProjectResponse>.Success(resultDto).ToMinimalApiResult();
            }
            
            return result.ToApiResponse().ToMinimalApiResult();
        });
    }
}