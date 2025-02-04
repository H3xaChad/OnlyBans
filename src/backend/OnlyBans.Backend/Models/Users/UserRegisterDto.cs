using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlyBans.Backend.Models.Users;

public class UserRegisterDto {
    
    [Required]
    [MaxLength(42)] 
    public required string Name;

    [Required]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
    public required string Email;
    
    [Required]
    [StringLength(20, MinimumLength = 7)]
    [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format.")]
    public required string PhoneNumber { get; init; }
    
    [Required]
    public required DateOnly BirthDate { get; init; }

    [Required]
    [MinLength(8)]
    [PasswordPropertyText]
    public required string Password;
}