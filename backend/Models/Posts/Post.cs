using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlyBans.Backend.Models.Comments;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Models.Posts;

public class Post {
    
    [Key]
    public Guid Id { get; init; }
    public ImageType ImageType { get; init; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
    public List<UserPostLike> LikedByUsers { get; init; } = [];
    public List<Comment> Comments { get; set; } = [];
}