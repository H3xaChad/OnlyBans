using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlyBans.Backend.Models;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Models.Posts;

public class Post {
    
    [Key]
    public Guid Id { get; init; }

    public string Title { get; init; } = null!;

    public string Text { get; init; } = null!;

    [ForeignKey(nameof(User))]
    public Guid UserId { get; init; }

    public User User { get; init; } = null!;

    public List<UserPostLike> LikedByUsers { get; set; } = [];
}