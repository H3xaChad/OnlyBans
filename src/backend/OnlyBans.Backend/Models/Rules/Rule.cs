using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Models.Rules;

public class Rule {
    
    [Key]
    public Guid Id { get; init; }
    
    public string Text { get; init; } = null!;
    
    public string RuleCategory { get; init; } = null!;
    
    [ForeignKey(nameof(User))]
    public Guid UserId { get; init; }

    public User User { get; init; } = null!;

    public Rule ToRule(Guid userId) {
        return new Rule {
            Text = Text,
            RuleCategory = RuleCategory,
            UserId = userId
        };
    }
}