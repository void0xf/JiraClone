#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public enum PrivacyLevel
    {
        Public,
        Private,
    }

    public class PrivacySetting
    {
        public string? Value { get; set; }
        public PrivacyLevel WhoCanSee { get; set; } = PrivacyLevel.Private;
    }
    
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string KeycloakUserId { get; set; } = null!;
        public PrivacySetting? FullName { get; set; }
        public PrivacySetting? PublicName { get; set; }
        public string HeaderImageUrl { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;   
        public List<Guid> WorkedOn { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}