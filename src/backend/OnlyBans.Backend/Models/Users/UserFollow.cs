using System.ComponentModel.DataAnnotations.Schema;

namespace OnlyBans.Backend.Models.Users;

public class UserFollow {
    [ForeignKey(nameof(Follower))]
    public Guid FollowerId { get; init; }

    [ForeignKey(nameof(Followed))]
    public Guid FollowedId { get; init; }

    public DateTime FollowedAt { get; set; } = DateTime.UtcNow;

    public User Follower { get; init; } = null!;
    public User Followed { get; init; } = null!;
}