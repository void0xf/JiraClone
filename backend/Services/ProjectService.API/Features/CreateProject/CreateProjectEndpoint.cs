using ProjectService.API.Features.CreateProject.DTO;

namespace ProjectService.API.Features.CreateProject;

public record CreateProjectRequest(ProjectRequestDto project);

public record CreateProjectResponse(Guid ProjectId);


public class CreateProjectEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/projects", 
            async (CreateProjectRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateProjectCommand>();
            
            var result = await sender.Send(command);
            
            var response = result.Adapt<CreateProjectResponse>();
            
            return Results.Created($"/projects/{response.ProjectId}", response);
        })
            .WithName("CreateProject")
            .Produces<CreateProjectResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Project")
            .WithDescription("Create Project");
    }
}