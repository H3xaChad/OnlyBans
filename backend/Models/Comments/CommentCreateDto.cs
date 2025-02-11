using System.ComponentModel.DataAnnotations;

namespace OnlyBans.Backend.Models.Comments;

public class CommentCreateDto {
    
    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public required string Content { get; init; }
    
    [Required]
    public required Guid PostId { get; init; }
    
    [Required]
    public required Guid UserId { get; init; }
}