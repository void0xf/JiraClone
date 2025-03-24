using Marten.Patching;
using ProjectService.API.Models;
using SharedKernel;
using SharedKernel.Utils;

namespace ProjectService.API.Features.UpdateProject;

// Command, Result, and Handler in the same file
public record UpdateProjectCommand(Project project) : IRequest<Result<UpdateProjectResult>>;

public record UpdateProjectResult(Guid ProjectId);

public class UpdateProjectByIdHandler(IDocumentSession session) : IRequestHandler<UpdateProjectCommand, Result<UpdateProjectResult>>
{
    public async Task<Result<UpdateProjectResult>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingProject = await session.Query<Project>()
                .FirstOrDefaultAsync(p => p.Id == request.project.Id, cancellationToken);
            
            if (existingProject == null)
            {
                return Result<UpdateProjectResult>.Failure(
                    Error.NotFound(ErrorCode.NotFound, "Project not found", "It's seems like your project does not exist"));
            }
            ObjectExtensions.CopyNonNullProperties(request.project, existingProject);
            
            
            session.Update(existingProject);
            await session.SaveChangesAsync(cancellationToken);
            
            return Result<UpdateProjectResult>.Success(new UpdateProjectResult(request.project.Id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<UpdateProjectResult>.Failure(
                Error.Unexpected(ErrorCode.UnknownError, "Failed to update project", "Failed to update project", e));
        }
    }
}