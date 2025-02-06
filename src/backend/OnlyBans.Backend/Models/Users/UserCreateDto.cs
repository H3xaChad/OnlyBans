using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlyBans.Backend.Models.Users;

public class UserCreateDto {
    
    [Required]
    [MinLength(1)]
    [MaxLength(42)]
    public required string UserName { get; init; }

    [Required]
    [DefaultValue("user@example.com")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
    public required string Email { get; init; }
    
    [Required]
    [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format.")]
    public required string PhoneNumber { get; init; }
    
    [Required]
    public required DateOnly BirthDate { get; init; }

    [Required]
    [MinLength(8)]
    [PasswordPropertyText]
    public required string Password { get; init; }

    public User ToUser() {
        return new User {
            UserName = UserName,
            Email = Email,
            PhoneNumber = PhoneNumber,
            BirthDate = BirthDate,
            State = UserState.Free
        };
    }
}