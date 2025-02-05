using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Data;
using OnlyBans.Backend.Models;
using OnlyBans.Backend.Models.Users;
using OnlyBans.Backend.Security;

namespace OnlyBans.Backend.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager) : ControllerBase {

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
            return Unauthorized(new { message = "Invalid credentials" });

        var passwordValid = await userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!passwordValid)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(new { message = "Login successful" });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserCreateDto userDto) {
        var result = await userManager.CreateAsync(userDto.ToUser(), userDto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = "User successful" });
    }
}