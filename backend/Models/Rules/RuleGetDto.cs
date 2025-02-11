using OnlyBans.Backend.Models.Posts;

namespace OnlyBans.Backend.Models.Rules;

public class RuleGetDto(Rule rule)
{
    public Guid Id { get; }
    
    public string Text { get; }
    
    public RuleEnum RuleCategory { get; }
    
    public Guid UserId { get; }
    
    public DateTime CreatedAt { get; }
}