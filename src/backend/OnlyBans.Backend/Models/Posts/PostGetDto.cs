namespace OnlyBans.Backend.Models.Posts;

public class PostGetDto(Post post) {

    public Guid Id { get; } = post.Id;

    public string Title { get; } = post.Title;

    public string Text { get; } = post.Text;

    public Guid UserId { get; } = post.UserId;
}