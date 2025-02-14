using Microsoft.AspNetCore.Identity;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Utils;

public class CustomUtils {
    
    public static async Task<bool> IsOAuthUser(UserManager<User> userManager, User user) {
        var externalLogins = await userManager.GetLoginsAsync(user);
        return externalLogins.Count > 0;
    }
}