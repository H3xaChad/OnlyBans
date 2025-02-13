using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Models.Comments;

public class Comment {
    
    [Key]
    public Guid Id { get; init; }
    public required string Content { get; init; }
    public required Guid PostId { get; init; }
    public required Post Post { get; init; }
    public required Guid UserId { get; init; }
    public required User User { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}