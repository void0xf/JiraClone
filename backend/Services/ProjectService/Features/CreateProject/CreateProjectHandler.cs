using System.Diagnostics;
using ProjectService.API.Models;
using SharedKernel;

namespace ProjectService.API.Features.CreateProject;

public record CreateProjectCommand(
    string Name, 
    string ProjectKey, 
    AccessLevel AccessLevel,
    ProjectTemplate  ProjectTemplate) : IRequest<Result<CreateProjectResult>>;

public record CreateProjectResult(Guid ProjectId);

public class CreateProjectHandler(IDocumentSession session) : IRequestHandler<CreateProjectCommand, Result<CreateProjectResult>>
{
    public async Task<Result<CreateProjectResult>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            ProjectKey = request.ProjectKey,
            AccessLevel = request.AccessLevel,
            ProjectTemplate = request.ProjectTemplate,
            CreatedAt = DateTime.UtcNow,
            LeadId = Guid.Empty,//empty due to lack of authorization at this point
            Members = new List<Guid>(),
            UpdatedAt = DateTime.UtcNow
        };
        try
        {
            var sp = Stopwatch.StartNew();
            sp.Start();
            session.Store(project);
            await session.SaveChangesAsync(cancellationToken);
            sp.Stop();
            Console.WriteLine($"Created project in {sp.ElapsedMilliseconds} ms");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<CreateProjectResult>.Failure(Error.Unexpected(ErrorCode.UnknownError, "Failed to create project", "Failed to create project", e));
        }
        
            
        return Result<CreateProjectResult>.Success(new CreateProjectResult(project.Id));
    }
}