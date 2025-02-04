using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Data;
using OnlyBans.Backend.Models;
using OnlyBans.Backend.Models.Users;
using OnlyBans.Backend.Security;

namespace OnlyBans.Backend.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController(AppDbContext context) : ControllerBase {
    
    private const string UserAvatarPath = "Uploads/Avatars";
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserGetDto>>> GetUsers() {
        var users = await context.Users.ToListAsync();
        return Ok(users.Select(user => new UserGetDto(user)));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserGetDto>> GetUser(Guid id) {
        var user = await context.Users.FindAsync(id);
        if (user == null)
            return NotFound();

        return Ok(new UserGetDto(user));
    }
    
    [HttpGet("{id:guid}/avatar")]
    public async Task<ActionResult<UserGetDto>> GetUserImage(Guid id) {
        var user = await context.Users.FindAsync(id);
        if (user == null)
            return NotFound();
            
        var imagePath = Path.Combine(UserAvatarPath, user.Id.ToString());
        if (!System.IO.File.Exists(imagePath))
            return NotFound();
        
        var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(imagePath, out var contentType)) {
            contentType = "application/octet-stream"; // Fallback MIME type
        }
            
        var image = System.IO.File.OpenRead(imagePath);
        return File(image, contentType);
    }
    
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody] UserCreateDto userDto) {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var emailExists = await context.Users.AnyAsync(u => u.Email == userDto.Email);
        if (emailExists)
            return Conflict(new { message = "Email is already in use." });

        var newUser = new User {
            Name = userDto.Name,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
            BirthDate = userDto.BirthDate,
            // isBanned = false,
            PasswordHash = PasswordHasher.HashPassword(userDto.Password)
        };
        
        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
    }
    
    
}