namespace OnlyBans.Backend.Models.Posts;

public class PostGetDto(Post post) {
    public Guid Id { get; } = post.Id;
    public Guid UserId { get; } = post.UserId;
    public string Title { get; } = post.Title;
    public string Description { get; } = post.Description;
    // TODO: This may cause a NullReferenceException
    // public int LikeCount { get; init; } = post.LikedByUsers.Count;
}