using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlyBans.Backend.Models;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Models.Posts;

public class Post {
    
    [Key]
    public Guid Id { get; init; }

    [Required]
    [MaxLength(42)]
    public required string Title { get; init; }

    [Required]
    [MaxLength(1600)]
    public required string Text { get; init; }

    [Required]
    public required Guid CreatorId { get; init; }

    [ForeignKey(nameof(CreatorId))]
    public required User Creator { get; init; }

    public List<UserPostLike> LikedByUsers { get; set; } = [];
}