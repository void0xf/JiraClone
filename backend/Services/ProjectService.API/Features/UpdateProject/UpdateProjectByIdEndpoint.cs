using ProjectService.API.Features.UpdateProject;
using ProjectService.API.Features.CreateProject.DTO;
using ProjectService.API.Models;
using SharedKernel;

namespace ProjectService.API.Features.UpdateProject;

public class UpdateProjectByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/projects/{id}", async (HttpContext context, ISender sender, Guid id) =>
        {
            var bodyData = await context.Request.ReadFromJsonAsync<ProjectRequestDto>();
            if (bodyData == null) return TypedResults.BadRequest("Invalid project data.");
            
            var project = bodyData.Adapt<Project>();
            project.Id = id;
            
            var result = await sender.Send(new UpdateProjectCommand(project));
            return result.ToMinimalApiResult();
        });
    }
}