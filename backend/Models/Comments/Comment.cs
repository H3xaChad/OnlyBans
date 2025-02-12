using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Models.Comments;

public class Comment {
    
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Content { get; init; }
    public required Guid PostId { get; init; }
    //[ForeignKey(nameof(PostId))]
    public required Post Post { get; init; }
    public required Guid UserId { get; init; }
    //[ForeignKey(nameof(UserId))]
    public required User User { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}