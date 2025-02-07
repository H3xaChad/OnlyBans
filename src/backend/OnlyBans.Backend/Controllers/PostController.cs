using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Data;
using OnlyBans.Backend.Models;
using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PostController(AppDbContext context, UserManager<User> userManager) : ControllerBase {
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserGetDto>>> GetPosts() {
        var posts = await context.Posts.ToListAsync();
        return Ok(posts.Select(post => new PostGetDto(post)));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserGetDto>> GetPost(Guid id) {
        var post = await context.Posts.FindAsync(id);
        if (post == null)
            return NotFound();

        return Ok(new PostGetDto(post));
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<UserGetDto>> CreatePost(PostCreateDto dto) {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var user = await userManager.GetUserAsync(User);
        var post = new Post {
            Title = dto.Title,
            Text = dto.Text,
            UserId = Guid.Empty
        };
        context.Posts.Add(post);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPost), new { id = post.Id }, new PostGetDto(post));
    }
    
    [HttpPost("{id:guid}/like")]
    public async Task<IActionResult> LikePost(Guid id) {
        var userId = Guid.Empty;

        var post = await context.Posts.FindAsync(id);
        if (post == null)
            return NotFound(new { message = "Post not found" });
        
        var existingLike = await context.UserPostLikes
            .FirstOrDefaultAsync(l => l.PostId == id && l.UserId == userId);

        if (existingLike != null) {
            context.UserPostLikes.Remove(existingLike);
            await context.SaveChangesAsync();
            return Ok(new { message = "Like removed successfully" });
        }
        
        var like = new UserPostLike {
            PostId = id,
            UserId = userId
        };
        context.UserPostLikes.Add(like);
        await context.SaveChangesAsync();
        return Ok(new { message = "Post liked successfully" });
    }

}