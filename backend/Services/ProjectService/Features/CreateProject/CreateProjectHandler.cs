using System.Diagnostics;
using System.Security.Claims;
using ProjectService.API.Models;
using SharedKernel;

namespace ProjectService.API.Features.CreateProject;

public record CreateProjectCommand(
    string Name, 
    string ProjectKey, 
    AccessLevel AccessLevel,
    ProjectTemplate  ProjectTemplate) : IRequest<Result<CreateProjectResult>>;

public record CreateProjectResult(Guid ProjectId);

public class CreateProjectHandler(IDocumentSession _session, IHttpContextAccessor _httpContextAccessor) : IRequestHandler<CreateProjectCommand, Result<CreateProjectResult>>
{

    public async Task<Result<CreateProjectResult>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var principal = _httpContextAccessor.HttpContext?.User;
        var keycloakUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (keycloakUserId == null)
        {
            return Result<CreateProjectResult>.Failure(Error.Conflict(ErrorCode.Forbidden, "User is not authenticated.", 
                "User is not authenticated"));
        }
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            ProjectKey = request.ProjectKey,
            AccessLevel = request.AccessLevel,
            ProjectTemplate = request.ProjectTemplate,
            CreatedAt = DateTime.UtcNow,
            LeadId = Guid.Parse(keycloakUserId),
            Members = new List<Guid>(),
            UpdatedAt = DateTime.UtcNow
        };
        try
        {
            var sp = Stopwatch.StartNew();
            sp.Start();
            _session.Store(project);
            await _session.SaveChangesAsync(cancellationToken);
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