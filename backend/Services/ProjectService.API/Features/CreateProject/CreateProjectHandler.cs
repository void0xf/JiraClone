using ProjectService.API.Models;

namespace ProjectService.API.Features.CreateProject;

public record CreateProjectCommand(
    string Name, 
    string ProjectKey, 
    AccessLevel AccessLevel,
    ProjectTemplate  ProjectTemplate) : IRequest<CreateProjectResult>;

public record CreateProjectResult(Guid ProjectId);

public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, CreateProjectResult>
{
    public async Task<CreateProjectResult> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            ProjectKey = request.ProjectKey,
            AccessLevel = request.AccessLevel,
            ProjectTemplate = request.ProjectTemplate,
            CreatedAt = DateTime.UtcNow,
            LeadId = Guid.Empty,
            Members = new List<Guid>(),
            UpdatedAt = DateTime.UtcNow
        };
        //save in db
        return new CreateProjectResult(project.Id);
    }
}