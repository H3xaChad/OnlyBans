using Microsoft.AspNetCore.Identity;
using OnlyBans.Backend.Utils;

namespace OnlyBans.Backend.Models.Users;

public class UserGetDto(User user, UserManager<User> userManager) {
    
    public Guid Id { get; } = user.Id;
    public string DisplayName { get; } = user.DisplayName!;
    public string UserName { get; } = user.UserName!;
    public string Email { get; } = user.Email!;
    public bool IsOAuthUser { get; } = CustomUtils.IsOAuthUser(userManager, user).Result;
}