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
    public required string Description { get; init; }
    
    [Required]
    public required IFormFile Image { get; init; }
    
    public Post ToPost(Guid userId, ImageType imageType) {
        return new Post {
            ImageType = imageType,
            Title = Title,
            Description = Description,
            UserId = userId
        };
    }
}