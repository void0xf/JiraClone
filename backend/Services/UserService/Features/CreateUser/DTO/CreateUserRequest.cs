using System.ComponentModel.DataAnnotations;

namespace UserService.Features.CreateUser.DTO;

public record CreateUserRequest
{
	[Required]
	[EmailAddress]
	public string Email { get; init; } = string.Empty;
}

