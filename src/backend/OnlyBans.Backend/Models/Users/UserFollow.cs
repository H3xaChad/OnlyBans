using System.ComponentModel.DataAnnotations.Schema;

namespace OnlyBans.Backend.Models.Users;

public class UserFollow {
    
    [ForeignKey(nameof(Follower))]
    public Guid FollowerId { get; init; }
    
    public User Follower { get; init; } = null!;

    [ForeignKey(nameof(Followed))]
    public Guid FollowedId { get; init; }
    
    public User Followed { get; init; } = null!;

    public DateTime FollowedAt { get; init; } = DateTime.UtcNow;
}