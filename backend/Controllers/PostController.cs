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
public class PostController(
    AppDbContext context,
    UserManager<User> userManager,
    IImageService imageService,
    CommentService commentService) : ControllerBase {
    
    [HttpGet("all", Name = "getAllPosts")]
    public async Task<ActionResult<IEnumerable<PostGetDto>>> GetPosts() {
        var posts = await context.Posts
            .AsNoTracking()
            .Include(p => p.User)
            .Select(p => new PostGetDto(p))
            .ToListAsync();

        return Ok(posts);
    }
    
    [Authorize]
    [HttpGet("me", Name = "getMyPosts")]
    public async Task<ActionResult<IEnumerable<PostGetDto>>> GetMyPosts() {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized("Please log in to create a post");
        
        var posts = await context.Posts
            .AsNoTracking()
            .Where(p => p.UserId == user.Id)
            .Include(p => p.User)
            .Select(p => new PostGetDto(p))
            .ToListAsync();

        return Ok(posts);
    }
    
    [HttpGet("{id:guid}", Name = "getPost")]
    public async Task<ActionResult<PostGetDto>> GetPost(Guid id) {
        var post = await context.Posts
            .AsNoTracking()
            .Include(p => p.User)
            .Where(p => p.Id == id)
            .Select(p => new PostGetDto(p))
            .FirstOrDefaultAsync();
    
        if (post == null)
            return NotFound("This post does not exist.");
    
        return Ok(post);
    }
    
    [HttpGet("{id:guid}/image", Name = "getPostImage")]
    public async Task<IActionResult> GetPostImage(Guid id) {
        var post = await context.Posts.FindAsync(id);
        if (post == null)
            return NotFound("This post does not exist.");

        return await imageService.GetPostImage(post);
    }
    
    [Authorize]
    [HttpPost(Name = "createPost")]
    [Consumes("multipart/form-data")]
    //[ProducesResponseType]
    public async Task<ActionResult<PostGetDto>> CreatePost([FromForm] PostCreateDto postDto) {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized("Please log in to create a post");

        var allowedExtensions = ImageTypeExtensions.GetFileExtensions();
        var fileExtension = Path.GetExtension(postDto.Image.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(fileExtension))
            return BadRequest(new { message = $"Invalid image format. Allowed formats are: {allowedExtensions}" });
        
        var post = postDto.ToPost(user.Id, ImageTypeExtensions.FromFileExtension(fileExtension));
        var vh = new ValidationHandler(context);
        context.Posts.Add(post);
        if (!vh.validateContent(post)) {
            context.Posts.Remove(post);
            return BadRequest("Post content is not valid");
        }

        await imageService.SavePostImageAsync(post, postDto.Image);

        context.Posts.Add(post);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPost), new { id = post.Id }, new PostGetDto(post));
    }
    
    [Authorize]
    [HttpPost("{id:guid}/like", Name = "likePost")]
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
    [HttpDelete("{id:guid}/like", Name = "unlikePost")]
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