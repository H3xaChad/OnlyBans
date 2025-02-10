using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Models.Posts;

public class PostCreateDto {

    [Required]
    [MaxLength(42)]
    public required string Title { get; init; }

    [Required]
    [MaxLength(1600)]
    public required string Text { get; init; }

    public Post ToPost(Guid userId) {
        return new Post {
            Title = Title,
            Text = Text,
            UserId = userId
        };
    }
}