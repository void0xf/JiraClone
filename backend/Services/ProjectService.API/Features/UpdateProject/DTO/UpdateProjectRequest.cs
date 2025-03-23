using ProjectService.API.Models;

namespace ProjectService.API.Features.UpdateProject.DTO;

public record UpdateProjectRequest(
    string Name, 
    ProjectTemplate ProjectTemplate, 
    string ProjectKey, 
    AccessLevel AccessLevel); 