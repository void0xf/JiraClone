using System.ComponentModel.DataAnnotations;

namespace UserService.Features.ResendVerifyMail.DTO;

public record ResendVerificationEmailRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;
}
