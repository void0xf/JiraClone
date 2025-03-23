using ProjectService.API.Features.UpdateProject.DTO;
using ProjectService.API.Models;
using SharedKernel;

namespace ProjectService.API.Features.UpdateProject;

public class UpdateProjectByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/projects/{id}", async (UpdateProjectRequest request, ISender sender, Guid id) =>
        {
            var project = new Project
            {
                Id = id,
                Name = request.Name,
                ProjectKey = request.ProjectKey,
                AccessLevel = request.AccessLevel,
                ProjectTemplate = request.ProjectTemplate,
                UpdatedAt = DateTime.UtcNow
            };
            
            var command = new UpdateProjectCommand(project);
            var result = await sender.Send(command);
            if (!result.IsSuccess)
                return result.ToApiResponse().ToMinimalApiResult();;
            
            var resultDto = result.Value.ProjectId.Adapt<UpdateProjectResponse>();
            return ApiResponse<UpdateProjectResponse>.Success(resultDto).ToMinimalApiResult();
            
            
        });
    }
}