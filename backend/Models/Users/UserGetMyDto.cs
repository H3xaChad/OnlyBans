using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using OnlyBans.Backend.Utils;

namespace OnlyBans.Backend.Models.Users;

public class UserGetMyDto(User user, UserManager<User> userManager) {
    
    public Guid Id { get; } = user.Id;
    public string Email { get; } = user.Email!;
    public string UserName { get; } = user.UserName!;
    public string DisplayName { get; } = user.DisplayName;
    public DateOnly BirthDate { get; } = user.BirthDate;
    public DateTime CreatedAt { get; } = user.CreatedAt;
    public bool IsOAuthUser { get; } = CustomUtils.IsOAuthUser(userManager, user).Result;
}