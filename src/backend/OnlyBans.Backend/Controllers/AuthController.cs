using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Data;
using OnlyBans.Backend.Models;
using OnlyBans.Backend.Models.Users;
using OnlyBans.Backend.Security;

namespace OnlyBans.Backend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager) : ControllerBase {

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
        var user = await context.Users.SingleOrDefaultAsync(u => loginDto.Email == u.Email);
        if (user == null || !PasswordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(new { message = "Login successful" });
    }
    
    
}