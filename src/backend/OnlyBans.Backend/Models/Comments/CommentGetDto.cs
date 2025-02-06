namespace OnlyBans.Backend.Models.Comments;

public class CommentGetDto(Comment comment) {
    
    public Guid Id { get; } = comment.Id;

    public Guid PostId { get; } = comment.PostId;
    
    public Guid UserId { get; } = comment.UserId;

    public string Content { get; } = comment.Content;

    public DateTime CreatedAt { get; } = comment.CreatedAt;
}