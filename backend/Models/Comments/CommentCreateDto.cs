using System.ComponentModel.DataAnnotations;

namespace OnlyBans.Backend.Models.Comments;

public class CommentCreateDto {
    
    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public required string Content { get; init; }
    
    [Required]
    public required Guid PostId { get; init; }
    
    public Comment ToComment(Guid userId) {
        return new Comment {
            UserId = userId,
            PostId = PostId,
            Content = Content
        };
    }
}