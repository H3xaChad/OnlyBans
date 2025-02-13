using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlyBans.Backend.Models.Users;

public class UserUpdateDto {
    
    [Required]
    [MinLength(1)]
    [MaxLength(42)]
    public required string UserName { get; init; }
    
    [Required]
    [MinLength(1)]
    [MaxLength(42)]
    public required string DisplayName { get; init; }

    [Required]
    [DefaultValue("user@example.com")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
    public required string Email { get; init; }
    
    [Required]
    [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format.")]
    public required string PhoneNumber { get; init; }

    [Required]
    [MinLength(8)]
    [PasswordPropertyText]
    public required string Password { get; init; }
}