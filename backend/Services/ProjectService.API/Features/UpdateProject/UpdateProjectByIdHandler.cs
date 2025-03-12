
using ProjectService.API.Models;

namespace ProjectService.API.Features.UpdateProject;

public record UpdateProjectCommand(Project project) : IRequest<UpdateProjectResult>;
public record UpdateProjectResult(Guid projectId);

public class UpdateProjectByIdHandler(IDocumentSession session) : IRequestHandler<UpdateProjectCommand, UpdateProjectResult>
{
    public async Task<UpdateProjectResult> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
       var project = await session.Query<Project>()
            .Where(p => p.Id == request.project.Id)
            .ToListAsync(cancellationToken);
       
       if (project.Count <= 0) return new UpdateProjectResult(Guid.Empty);
       
       session.Update(request.project);
       await session.SaveChangesAsync(cancellationToken);
       return new UpdateProjectResult(request.project.Id);
    }
}