using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Database;
using OnlyBans.Backend.Models;
using OnlyBans.Backend.Models.Comments;
using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Users;
using OnlyBans.Backend.Services;
using OnlyBans.Backend.Spine.Validation;

namespace OnlyBans.Backend.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PostController(AppDbContext context, UserManager<User> userManager, CommentService commentService) : ControllerBase {
    
    [HttpGet(Name = "getAll")]
    public async Task<ActionResult<IEnumerable<UserGetDto>>> GetPosts() {
        var posts = await context.Posts.ToListAsync();
        return Ok(posts.Select(post => new PostGetDto(post)));
    }
    
    [HttpGet("{id:guid}", Name = "getPost")]
    public async Task<ActionResult<UserGetDto>> GetPost(Guid id) {
        var post = await context.Posts.FindAsync(id);
        if (post == null)
            return NotFound();

        return Ok(new PostGetDto(post));
    }
    
    [Authorize]
    [HttpPost(Name = "create")]
    public async Task<ActionResult<UserGetDto>> CreatePost(PostCreateDto postDto) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized("Please log in to create a post");
        var post = postDto.ToPost(user.Id);
        var vh = new ValidationHandler(context);
        if (!vh.validateContent(post)) 
            return BadRequest("Post content is not valid");
        context.Posts.Add(post);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
    }
    
    [Authorize]
    [HttpPost("{id:guid}/like", Name = "like")]
    public async Task<IActionResult> LikePost(Guid id) {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized("Please log in to like a post");
        if (!await context.Posts.AnyAsync(p => p.Id == id))
            return NotFound(new { message = "Post not found" });

        var alreadyLiked = await context.UserPostLikes.AnyAsync(l => l.PostId == id && l.UserId == user.Id);
        if (alreadyLiked) 
            return BadRequest(new { message = "Post already liked" });

        context.UserPostLikes.Add(new UserPostLike { PostId = id, UserId = user.Id });
        await context.SaveChangesAsync();
        return Ok(new { message = "Post liked successfully" });
    }

    [Authorize]
    [HttpDelete("{id:guid}/like", Name = "unlike")]
    public async Task<IActionResult> UnlikePost(Guid id) {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized("Please log in to unlike a post");

        var like = await context.UserPostLikes.FirstOrDefaultAsync(l => l.PostId == id && l.UserId == user.Id);
        if (like == null)
            return NotFound(new { message = "Like not found" });

        context.UserPostLikes.Remove(like);
        await context.SaveChangesAsync();
        return Ok(new { message = "Like removed successfully" });
    }
    
    [HttpGet("{postId:guid}/comments", Name = "getComments")]
    public async Task<ActionResult<IEnumerable<CommentGetDto>>> GetPostComments(Guid postId) {
        return Ok(await commentService.GetCommentsByPost(postId));
    }

}