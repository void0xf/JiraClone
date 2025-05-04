using ProjectService.API.Features.CreateProject.DTO;
using SharedKernel;

namespace ProjectService.API.Features.CreateProject;

public class CreateProjectEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/projects",
                async (CreateProjectRequest request, ISender sender, LinkGenerator links) =>
                {
                    var command = new CreateProjectCommand(
                        request.Name,
                        request.ProjectKey,
                        request.AccessLevel,
                        request.ProjectTemplate);
                        
                    var result = await sender.Send(command);

                    if (result.IsSuccess)
                    {
                        var apiResponse = result.ToApiResponse();
                        return apiResponse.
                            ToCreatedResult($"/projects/{new CreateProjectResponse(result.Value.ProjectId)}");
                    }

                    return result.ToApiResponse().ToMinimalApiResult();
                })
            .WithName("CreateProject")
            .Produces<ApiResponse<CreateProjectResponse>>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Project");
    }
}