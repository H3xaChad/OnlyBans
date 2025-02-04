using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Models;

public class UserPostLike {
    public required Guid UserId { get; init; }
    public required User User { get; init; }

    public required Guid PostId { get; init; }
    public required Post Post { get; init; }
}