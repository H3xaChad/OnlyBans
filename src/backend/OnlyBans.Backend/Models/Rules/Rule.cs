using System.ComponentModel.DataAnnotations;

namespace OnlyBans.Backend.Models.Rules;

public class Rule {
    
    [Key]
    public Guid Id { get; init; }
    
    public string Text { get; init; } = null!;
    
    public string RuleCategory { get; init; } = null!;
}