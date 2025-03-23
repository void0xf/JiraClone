using ProjectService.API.Models;

namespace ProjectService.API.Features.CreateProject.DTO;

public record CreateProjectRequest(
    string Name, 
    ProjectTemplate ProjectTemplate, 
    string ProjectKey, 
    AccessLevel AccessLevel); 