using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Database;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class FollowController(AppDbContext context, UserManager<User> userManager) : ControllerBase {
    
    [Authorize]
    [HttpPost("{id:guid}", Name = "follow")]
    public async Task<IActionResult> FollowUser(Guid id) {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized("Please log in to follow users");

        if (id == user.Id)
            return BadRequest(new { message = "You cannot follow yourself" });

        var alreadyFollowing = await context.UserFollows
            .AnyAsync(f => f.FollowerId == user.Id && f.FollowedId == id);

        if (alreadyFollowing)
            return BadRequest(new { message = "You are already following this user" });

        context.UserFollows.Add(new UserFollow { FollowerId = user.Id, FollowedId = id });
        await context.SaveChangesAsync();
        return Ok(new { message = "User followed successfully" });
    }
    
    [Authorize]
    [HttpDelete("{id:guid}", Name = "unfollow")]
    public async Task<IActionResult> UnfollowUser(Guid id) {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized("Please log in to unfollow users");

        var follow = await context.UserFollows
            .FirstOrDefaultAsync(f => f.FollowerId == user.Id && f.FollowedId == id);

        if (follow == null)
            return NotFound(new { message = "You are not following this user" });

        context.UserFollows.Remove(follow);
        await context.SaveChangesAsync();
        return Ok(new { message = "User unfollowed successfully" });
    }
    
    [Authorize]
    [HttpGet("followers", Name = "getMyFollowers")]
    public async Task<ActionResult<IEnumerable<Guid>>> GetMyFollowers() {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized("You need to login for this operation.");
        var followers = await context.UserFollows
            .Where(f => f.FollowedId == user.Id)
            .Select(f => f.FollowerId)
            .ToListAsync();

        return Ok(followers);
    }
    
    [Authorize]
    [HttpGet("following", Name = "getMyFollowing")]
    public async Task<ActionResult<IEnumerable<Guid>>> GetMyFollowing() {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized("You need to login for this operation.");
        var following = await context.UserFollows
            .Where(f => f.FollowerId == user.Id)
            .Select(f => f.FollowedId)
            .ToListAsync();

        return Ok(following);
    }
}