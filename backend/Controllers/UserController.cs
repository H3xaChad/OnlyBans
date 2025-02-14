using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Database;
using OnlyBans.Backend.Models.Users;
using OnlyBans.Backend.Services;

namespace OnlyBans.Backend.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController(
    AppDbContext context,
    UserManager<User> userManager,
    IImageService imageService) : ControllerBase {
    
    [Authorize]
    [HttpGet("me", Name = "me")]
    public async Task<ActionResult<UserGetMyDto>> GetCurrentUser() {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized("You need to login for this operation.");
        return Ok(new UserGetMyDto(user, userManager));
    }
    
    [HttpGet(Name = "getAllUsers")]
    public async Task<ActionResult<IEnumerable<UserGetDto>>> GetUsers() {
        var users = await context.Users.ToListAsync();
        return Ok(users.Select(user => new UserGetDto(user)));
    }
    
    [HttpGet("{id:guid}", Name = "getUser")]
    public async Task<ActionResult<UserGetDto>> GetUser(Guid id) {
        var user = await context.Users.FindAsync(id);
        if (user == null) return NotFound();
        return Ok(new UserGetDto(user));
    }
    
    [Authorize]
    [HttpPatch("update", Name = "updateUser")]
    public async Task<ActionResult<UserGetMyDto>> Update([FromBody] UserUpdateDto updateDto) {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized("User not found.");
        
        user.UserName = updateDto.UserName;
        user.DisplayName = updateDto.DisplayName;
        user.Email = updateDto.Email;
        user.PhoneNumber = updateDto.PhoneNumber;
        user.BirthDate = updateDto.BirthDate;
        
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var passwordResult = await userManager.ResetPasswordAsync(user, token, updateDto.Password);
        if (!passwordResult.Succeeded)
            return BadRequest(passwordResult.Errors);
        
        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return BadRequest(updateResult.Errors);

        return Ok(new UserGetMyDto(user, userManager));
    }
    
    [Authorize]
    [HttpGet("avatar", Name = "getMyAvatar")]
    public async Task<IActionResult> GetMyAvatar() {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return NotFound("You need to login for this operation.");
        return await imageService.GetAvatarAsync(user);
    }
    
    [Authorize]
    [HttpPost("avatar", Name = "updateMyAvatar")]
    public async Task<IActionResult> UpdateMyAvatar(IFormFile image) {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return NotFound("You need to login for this operation.");

        await imageService.UpdateLocalAvatarAsync(user, image);
        return Ok("Successfully updated user image");
    }
    
    [HttpGet("{id:guid}/avatar", Name = "getAvatar")]
    public async Task<IActionResult> GetUserAvatar(Guid id) {
        var user = await context.Users.FindAsync(id);
        if (user == null) return NotFound("User with this id does not exist.");
        return await imageService.GetAvatarAsync(user);
    }
}