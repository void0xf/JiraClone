using ProjectService.API.Features.CreateProject;
using ProjectService.API.Models;

namespace ProjectService.API.Features.RemoveProject;

public record RemoveProjectCommand(Guid ProjectId) : IRequest<ReemoveProjectResult>; 

public record ReemoveProjectResult(Guid ProjectId);

public class RemoveProjectByIdHandler(IDocumentSession session) : IRequestHandler<RemoveProjectCommand, ReemoveProjectResult>
{
    public async Task<ReemoveProjectResult> Handle(RemoveProjectCommand request, CancellationToken cancellationToken)
    {
        var projectToDelete = await session.Query<Project>()
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
        if (projectToDelete != null)
        {
            session.Delete<Project>(projectToDelete);
            await session.SaveChangesAsync(cancellationToken);
            return new ReemoveProjectResult(projectToDelete.Id);
        }
        return new ReemoveProjectResult(Guid.Empty);
    }
}