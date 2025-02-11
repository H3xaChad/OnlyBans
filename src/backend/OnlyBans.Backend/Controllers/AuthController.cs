using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using OnlyBans.Backend.Models;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(UserManager<User> userManager, SignInManager<User> signInManager) : ControllerBase {
    
    [HttpPost("register", Name = "register")]
    public async Task<IActionResult> Register([FromBody] UserCreateDto userDto) {
        var result = await userManager.CreateAsync(userDto.ToUser(), userDto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = "User creation successful" });
    }
    
    [HttpPost("login", Name = "login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
            return Unauthorized(new { message = "This email is not registered, maybe a typo? :)" });

        var result = await signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
        if (!result.Succeeded)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(new {
            userId = user.Id,
            message = "Login successful"
        });
    }
    
    [HttpGet("login/{providerName}", Name = $"login-oauth2")]
    public IActionResult Login(string providerName, string? returnUrl = null) {
        var redirectUrl = Url.Action("ExternalCallback", new { returnUrl });
        var properties = signInManager.ConfigureExternalAuthenticationProperties(providerName, redirectUrl);
        return new ChallengeResult(providerName, properties);
    }
    
    [Authorize]
    [HttpPost("logout", Name = "logout")]
    public async Task<IActionResult> Logout() {
        await signInManager.SignOutAsync();
        return Ok(new { message = "User creation successful" });
    }
    
    [AllowAnonymous]
    [HttpGet("external/callback")]
    public async Task<IActionResult> ExternalCallback(string? returnUrl = null) {
        if (User.Identity is { IsAuthenticated: true }) await signInManager.SignOutAsync();

        ExternalLoginInfo? info;
        try {
            info = await signInManager.GetExternalLoginInfoAsync();
        }
        catch (Exception e) {
            //_logger.LogError(e, "Error getting external login info");
            return StatusCode(500);
        }
        
        if (info == null) {
            //_logger.LogInformation("Info is null");
            return BadRequest("Login info is null");
        }

        var providerKey = info.ProviderKey;
        if (info.LoginProvider == "bosch") {
            providerKey = info.Principal.FindFirst(ClaimConstants.ObjectId)?.Value;
        }

        // Attempt external sign-in.
        var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, providerKey, isPersistent: true, bypassTwoFactor: true);
        if (signInResult.Succeeded) {
            return returnUrl != null ? Redirect(returnUrl) : NoContent();
        }

        // Minimal: get the user's email from the claims.
        var email = info.Principal.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) {
            return BadRequest("Email claim missing.");
        }
        
        var claims = info.Principal.Claims.ToList();

        // Create a new user.
        var user = new User {
            Email = email,
            UserName = claims.SingleOrDefault(x => x.Type == ClaimConstants.Name)?.Value,
            BirthDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        var createResult = await userManager.CreateAsync(user);
        if (!createResult.Succeeded) {
            return BadRequest("User creation failed.");
        }

        // Add the external login info to the newly created user.
        var loginInfo = new UserLoginInfo(info.LoginProvider, providerKey, info.ProviderDisplayName);
        var addLoginResult = await userManager.AddLoginAsync(user, loginInfo);
        if (!addLoginResult.Succeeded) {
            return BadRequest("Adding external login failed.");
        }

        // Sign in the new user.
        await signInManager.SignInAsync(user, isPersistent: true, info.LoginProvider);
        return returnUrl != null ? Redirect(returnUrl) : NoContent();
    }
    
}