using ProjectService.API.Models;

namespace ProjectService.API.Features.CreateProject.DTO;

public class ProjectRequestDto
{ 
        public string Name { get; set; } = string.Empty;
        public ProjectTemplate ProjectTemplate { get; set; }
        public string ProjectKey { get; set; } = string.Empty;
        public AccessLevel AccessLevel { get; set; }
}