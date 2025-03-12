using System;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using ProjectService.API.Models;
using SharedKernel;

namespace ProjectService.API.Features.RemoveProject;

// Command, Result, and Handler in the same file
public record RemoveProjectCommand(Guid ProjectId) : IRequest<Result<RemoveProjectResult>>;

public record RemoveProjectResult(Guid ProjectId);

public class RemoveProjectByIdHandler(IDocumentSession session) : IRequestHandler<RemoveProjectCommand, Result<RemoveProjectResult>>
{
    public async Task<Result<RemoveProjectResult>> Handle(RemoveProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var projectToDelete = await session.Query<Project>()
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
                
            if (projectToDelete == null)
            {
                return Result<RemoveProjectResult>.Failure(
                    Error.NotFound(ErrorCode.NotFound, "Project not found", "Project not found"));
            }

            session.Delete(projectToDelete);
            await session.SaveChangesAsync(cancellationToken);
            return Result<RemoveProjectResult>.Success(new RemoveProjectResult(projectToDelete.Id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<RemoveProjectResult>.Failure(
                Error.Unexpected(ErrorCode.UnknownError, "Failed to remove project", "Failed to remove project", e));
        }
    }
}