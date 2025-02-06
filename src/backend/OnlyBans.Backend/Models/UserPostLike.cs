using System.ComponentModel.DataAnnotations.Schema;
using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Models;

public class UserPostLike {
    
    [ForeignKey(nameof(User))]
    public Guid UserId { get; init; }

    public User User { get; init; } = null!;

    [ForeignKey(nameof(Post))]
    public Guid PostId { get; init; }
    
    public Post Post { get; init; } = null!;
}