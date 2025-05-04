using UserService.Models;

namespace UserService.Features.GetUser.DTO;

public record PrivacySettingDto
{
    public string Value { get; set; }
    public PrivacyLevel WhoCanSee { get; set; }
}

public record GetUserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public PrivacySettingDto FullName { get; set; } 
    public PrivacySettingDto PublicName { get; set; }
    public string HeaderImageUrl { get; set; } = string.Empty;
    public string ProfilePictureUrl { get; set; } = string.Empty;   
    
    public List<Guid> WorkedOn { get; set; }
} 