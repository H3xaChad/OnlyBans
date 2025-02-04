using System.ComponentModel.DataAnnotations;
using OnlyBans.Backend.Models.Comments;
using OnlyBans.Backend.Models.Posts;

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

namespace OnlyBans.Backend.Models.Users;

public class User {
    
    [Key] 
    public Guid Id;
    
    public required string Name { get; init; }
    
    public required string Email { get; init; }
    
    public required string PhoneNumber { get; init; }
    
    public required DateOnly BirthDate { get; init; }
    
    public required string PasswordHash { get; init; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    public List<Post> Posts { get; init; } = [];
    public List<Comment> Comments { get; init; } = [];
    public List<UserPostLike> LikedPosts { get; init; } = [];
}