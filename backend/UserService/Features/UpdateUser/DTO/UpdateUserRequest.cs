using UserService.Models;

namespace UserService.Features.UpdateUser.DTO;

public record PrivacySettingDto
{
    public string Value { get; set; }
    public PrivacyLevel WhoCanSee { get; set; }
}

public record UpdateUserRequest
{
    public PrivacySettingDto FullName { get; set; }
    public PrivacySettingDto PublicName { get; set; }
    public string HeaderImageUrl { get; set; } = string.Empty;
    public string ProfilePictureUrl { get; set; } = string.Empty;
    public List<Guid> WorkedOn { get; set; } = new();
} 