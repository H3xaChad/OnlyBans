using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Rules;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Models.Rules;

public class RuleCreateDto {
    
    public string Text { get; init; } = null!;
    
    public RuleEnum RuleCategory { get; init; }
    
    public Rule ToRule(Guid userId) {
        return new Rule {
            Text = Text,
            RuleCategory = RuleCategory,
            UserId = userId,
        };
    }
}