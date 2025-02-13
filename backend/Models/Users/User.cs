using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using OnlyBans.Backend.Models.Comments;
using OnlyBans.Backend.Models.Posts;

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

namespace OnlyBans.Backend.Models.Users;

public class User : IdentityUser<Guid> {
    
    public string DisplayName { get; set; }
    public UserState State { get; set; } = UserState.Free;
    public DateOnly BirthDate { get; set; }
    public ImageType ImageType { get; set; } = ImageType.None;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public List<Post> Posts { get; init; } = [];
    public List<Comment> Comments { get; init; } = [];
    public List<UserPostLike> LikedPosts { get; init; } = [];
}