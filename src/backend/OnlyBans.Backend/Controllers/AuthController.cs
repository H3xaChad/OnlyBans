using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlyBans.Backend.Models;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(UserManager<User> userManager, SignInManager<User> signInManager) : ControllerBase {

    [HttpPost("login")]
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

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserCreateDto userDto) {
        var result = await userManager.CreateAsync(userDto.ToUser(), userDto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = "User creation successful" });
    }
}