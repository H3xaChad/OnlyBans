using System.Diagnostics;
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
        Debug.WriteLine($"======= We got the return url: {returnUrl}");
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
    [HttpGet("external/callback", Name = "external-callback")]
    public async Task<IActionResult> ExternalCallback(string? returnUrl = null) {
        // Sign out if already authenticated
        if (User.Identity is { IsAuthenticated: true }) 
            await signInManager.SignOutAsync();

        ExternalLoginInfo? info;
        try {
            info = await signInManager.GetExternalLoginInfoAsync();
        }
        catch (Exception e) {
            Debug.WriteLine(e, "Error getting external login info");
            return StatusCode(500);
        }
        if (info == null) {
            Debug.WriteLine("============ Info is null");
            return BadRequest();
        }
        
        if (info.AuthenticationTokens != null) {
            foreach (var token in info.AuthenticationTokens) {
                Debug.WriteLine($"=== Token: {token.Name} = {token.Value}");
            }
        } else {
            Debug.WriteLine("============ No tokens received from the provider.");
        }

        // Use a custom provider key if needed
        var providerKey = info.ProviderKey;
        if (info.LoginProvider == "bosch")
            providerKey = info.Principal.Claims.Single(x => x.Type == ClaimConstants.ObjectId).Value;

        var claims = info.Principal.Claims.ToList();
        foreach (var claim in claims) {
            Debug.WriteLine($"Claim: {claim.Type} = {claim.Value}");
        }

        var firstNameClaim = claims.SingleOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
        var lastNameClaim = claims.SingleOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
        var displayNameClaim = claims.SingleOrDefault(x => x.Type == ClaimConstants.Name)?.Value;

        if (string.IsNullOrEmpty(firstNameClaim) || string.IsNullOrEmpty(lastNameClaim) ||
            string.IsNullOrEmpty(displayNameClaim))
            return UnprocessableEntity();

        // Try signing in with external login info
        var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, providerKey, true, true);
        if (result.Succeeded) {
            var externalUser = await userManager.FindByLoginAsync(info.LoginProvider, providerKey);
            if (externalUser == null)
                return StatusCode(500);

            // Update tokens automatically for existing users
            await signInManager.UpdateExternalAuthenticationTokensAsync(info);

            // Refresh the sign-in (optional, but used here to update the cookie)
            await signInManager.SignOutAsync();
            await signInManager.SignInAsync(externalUser, isPersistent: true, info.LoginProvider);
            return returnUrl != null ? Redirect(returnUrl) : NoContent();
        }

        if (result.IsLockedOut) {
            Debug.WriteLine("============ User locked out");
            return BadRequest("Locked Out");
        }

        // If user is already signed in (should never be the case)
        if (User.Identity is { IsAuthenticated: true }) {
            Debug.WriteLine("============ User is already authenticated");
            return BadRequest();
        }

        // New user registration flow
        var user = new User {
            Email = claims.Single(x => x.Type == ClaimTypes.Email).Value,
            UserName = claims.SingleOrDefault(x => x.Type == ClaimConstants.PreferredUserName)?.Value,
            DisplayName = displayNameClaim,
            BirthDate = DateOnly.FromDateTime(DateTime.UtcNow),
            ImageType = ImageType.Remote,
            // Optionally set first and last name here if desired
        };

        var userCreateResult = await userManager.CreateAsync(user);
        if (!userCreateResult.Succeeded) {
            Debug.WriteLine("============ User create did not succeed");
            return BadRequest();
        }

        var addedLoginResult = await userManager.AddLoginAsync(user,
            new UserLoginInfo(info.LoginProvider, providerKey, info.ProviderDisplayName));
        if (!addedLoginResult.Succeeded) {
            Debug.WriteLine("============ User add login result did not succeed");
            return BadRequest();
        }
        
        await signInManager.UpdateExternalAuthenticationTokensAsync(info);

        // Tokens for new users are saved automatically if SaveTokens is enabled.
        await signInManager.SignInAsync(user, isPersistent: true, info.LoginProvider);
        return returnUrl != null ? Redirect(returnUrl) : NoContent();
    }

}