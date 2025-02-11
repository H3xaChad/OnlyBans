using OnlyBans.Backend.Models.Posts;

namespace OnlyBans.Backend.Models.Rules;

public class RuleGetDto(Rule rule)
{
    public Guid Id { get; } = rule.Id;
    
    public string Text { get; } = rule.Text;
    
    public RuleEnum RuleCategory { get; } = rule.RuleCategory;
    
    public Guid UserId { get; } = rule.UserId;

    public DateTime CreatedAt { get; } = rule.CreatedAt;
}