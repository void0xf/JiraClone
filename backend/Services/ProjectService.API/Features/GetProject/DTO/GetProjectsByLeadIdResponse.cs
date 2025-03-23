using ProjectService.API.Models;

namespace ProjectService.API.Features.GetProject.DTO;

public record GetProjectsByLeadIdResponse(IReadOnlyList<Project> Projects); 