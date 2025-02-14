using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Database;
using OnlyBans.Backend.Models.Comments;
using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CommentController(AppDbContext context, UserManager<User> userManager) : ControllerBase
{
    [Authorize]
    [HttpGet("{id:guid}", Name = "getComment")]
    public async Task<ActionResult<CommentGetDto>> GetComment(Guid id)
    {
        var comment = await context.Comments.FindAsync(id);
        if (comment == null)
            return NotFound("No comment with this id exists.");

        return Ok(new CommentGetDto(comment));
    }

    [Authorize]
    [HttpDelete("{id:guid}", Name = "deleteComment")]
    public async Task<IActionResult> DeleteComment(Guid id) {
        var comment = await context.Comments.FindAsync(id);
        if (comment == null)
            return NotFound("No comment with this id exists.");

        context.Comments.Remove(comment);
        await context.SaveChangesAsync();
        return NoContent();
    }
    
    [Authorize]
    [HttpPost(Name = "createComment")]
    public async Task<IActionResult> CreateComment(CommentCreateDto commentDto) {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized("Please log in to comment on a post");

        var post = await context.Posts.FindAsync(commentDto.PostId);
        if (post == null)
            return NotFound(new { message = "Post not found" });

        if (string.IsNullOrWhiteSpace(commentDto.Content))
            return BadRequest(new { message = "Comment content cannot be empty" });

        var comment = commentDto.ToComment(user.Id);
        context.Comments.Add(comment);
        await context.SaveChangesAsync();
        return Ok(new { message = "Comment posted successfully" });
    }

    [Authorize]
    [HttpPatch(Name = "changeComment")]
    public async Task<IActionResult> ChangeComment([FromBody] CommentUpdateDto commentDto) {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized("Please log in to change your comments");
        
        var comment = await context.Comments.FindAsync(commentDto.Id);
        if (comment == null)
            return NotFound("No comment with this id exists.");

        if (comment.UserId != user.Id) {
            return Unauthorized("This is not your comment!");
        }

        if (string.IsNullOrWhiteSpace(commentDto.Content))
            return BadRequest("Comment content cannot be empty.");

        comment.Content = commentDto.Content;
        context.Comments.Update(comment);
        await context.SaveChangesAsync();
        return Ok(new CommentGetDto(comment));
    }
}