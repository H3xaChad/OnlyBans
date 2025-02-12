using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using OnlyBans.Backend.Models.Comments;
using OnlyBans.Backend.Models.Posts;

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

namespace OnlyBans.Backend.Models.Users;

public class User : IdentityUser<Guid> {
    
    public string DisplayName { get; init; }

    public UserState State { get; init; } = UserState.Free;
    
    public required DateOnly BirthDate { get; init; }

    public ImageType ImageType { get; init; } = ImageType.None;

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    public List<Post> Posts { get; init; } = [];
    
    public List<Comment> Comments { get; init; } = [];
    
    public List<UserPostLike> LikedPosts { get; init; } = [];
}