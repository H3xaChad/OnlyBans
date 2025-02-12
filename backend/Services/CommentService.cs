using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Database;
using OnlyBans.Backend.Models.Comments;

namespace OnlyBans.Backend.Services;

public class CommentService(AppDbContext context) {
    
    public async Task<IEnumerable<CommentGetDto>> GetCommentsByPost(Guid postId) {
        var post = await context.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == postId);
        
        return post == null ? [] : post.Comments.Select(c => new CommentGetDto(c));
    }
}