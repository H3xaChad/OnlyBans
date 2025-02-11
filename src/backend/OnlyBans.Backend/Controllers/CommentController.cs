using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Database;
using OnlyBans.Backend.Models.Comments;
using OnlyBans.Backend.Models.Posts;

namespace OnlyBans.Backend.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CommentController(AppDbContext context) : ControllerBase {
    
    [HttpGet("{id:guid}", Name = "getComment")]
    public async Task<ActionResult<CommentGetDto>> GetComment(Guid id) {
        var comment = await context.Comments.FindAsync(id);
        if (comment == null)
            return NotFound();
        
        return Ok(new CommentGetDto(comment));
    }
    
    // [HttpGet("/by-post/{id:guid}")]
    // public async Task<ActionResult<IEnumerable<CommentGetDto>>> GetComments(Guid id) {
    //     var comment = await context.Comments.FindAsync();
    //     return Ok(posts.Select(post => new PostGetDto(post)));
    // }
    
}