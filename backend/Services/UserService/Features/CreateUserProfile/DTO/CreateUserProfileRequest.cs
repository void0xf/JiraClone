using System.ComponentModel.DataAnnotations;
using UserService.Models;

namespace UserService.Features.CreateUserProfile.DTO;

public record CreateUserProfileRequest
{
  
    [Required]
    public string FullName { get; init; } = null!;

    [Required]
    public string PublicName { get; init; } = null!;

}
