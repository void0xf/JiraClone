using ProjectService.API.Features.CreateProject.DTO;
using ProjectService.API.Models;

namespace ProjectService.API.Features.UpdateProject;

public class UpdateProjectByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/projects/{id}", async (HttpContext context, ISender sender, Guid id) =>
        {
            var bodyData = await context.Request.ReadFromJsonAsync<ProjectRequestDto>();
            if (bodyData == null) return Guid.Empty;

            var project = bodyData.Adapt<Project>();
            project.Id = id;
            var responese = await sender.Send(new UpdateProjectCommand(project));
            
            return responese.projectId;
        });
    }
}