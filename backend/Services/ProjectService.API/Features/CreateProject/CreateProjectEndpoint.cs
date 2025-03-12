using ProjectService.API.Features.CreateProject.DTO;
using SharedKernel;

namespace ProjectService.API.Features.CreateProject;

public record CreateProjectRequest(ProjectRequestDto project);

public record CreateProjectResponse(Guid ProjectId);


public class CreateProjectEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/projects",
                async (CreateProjectRequest request, ISender sender, LinkGenerator links) =>
                {
                    var command = request.Adapt<CreateProjectCommand>();
                    var result = await sender.Send(command);

                    if (result.IsSuccess)
                    {
                        ;
                        //var locationUri = links.GetUriByName(, new { id = result.Value.ProjectId });
                        return TypedResults.Created($"/projects/{result.Value.ProjectId}", result.Value.Adapt<CreateProjectResponse>());
                    }

                    return result.ToMinimalApiResult();


                })
            .WithName("CreateProject")
            .Produces<CreateProjectResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Project");
    }
}