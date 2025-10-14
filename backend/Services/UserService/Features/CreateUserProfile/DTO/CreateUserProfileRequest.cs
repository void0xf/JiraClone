using System.ComponentModel.DataAnnotations;
using UserService.Models;

namespace UserService.Features.CreateUserProfile.DTO;

public record CreateUserProfileRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    public string FullName { get; init; } = null!;

    [Required]
    public string PublicName { get; init; } = null!;

}
