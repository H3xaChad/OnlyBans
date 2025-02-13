using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Models.Comments;

public class Comment {
    
    [Key]
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public User User { get; init; }
    public Guid PostId { get; init; }
    public Post Post { get; init; }
    public string Content { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}