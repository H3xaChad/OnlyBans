using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Data;
using OnlyBans.Backend.Models.Users;

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
    public async Task<ActionResult<UserGetDto>> GetUserAvatar(Guid id) {
        var user = await context.Users.FindAsync(id);
        if (user == null)
            return NotFound();
            
        var imagePath = Path.Combine(UserAvatarPath, user.Id.ToString());
        if (!System.IO.File.Exists(imagePath))
            return NotFound();
        
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(imagePath, out var contentType)) {
            contentType = "application/octet-stream";
        }
            
        var image = System.IO.File.OpenRead(imagePath);
        return File(image, contentType);
    }
}