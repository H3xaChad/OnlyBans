using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Database;
using OnlyBans.Backend.Models.Comments;
using OnlyBans.Backend.Models.Posts;

namespace OnlyBans.Backend.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CommentController(AppDbContext context) : ControllerBase
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
    [HttpPatch("{postId:guid}", Name = "changeComment")]
    public async Task<IActionResult> ChangeComment(Guid postId, [FromBody] CommentUpdateDto commentDto) {
        var comment = await context.Comments.FindAsync(commentDto.Id);
        if (comment == null)
            return NotFound("No comment with this id exists.");

        if (comment.PostId != postId)
            return BadRequest("Post Ids do not match!");

        if (string.IsNullOrWhiteSpace(commentDto.Content))
            return BadRequest("Comment content cannot be empty.");

        comment.Content = commentDto.Content;
        context.Comments.Update(comment);
        await context.SaveChangesAsync();
        return Ok(new CommentGetDto(comment));
    }
}