using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Data;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController(AppDbContext context) : ControllerBase {
    
    private const string CustomerImageFolder = "Uploads/Avatar";
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserGetDto>>> GetUsers() {
        var users = await context.Users.ToListAsync();
        return Ok(users);
    }
}