using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Models.Rules;

public class Rule {
    
    [Key]
    public Guid Id { get; init; }
    public string Text { get; init; } = null!;
    public RuleEnum RuleCategory { get; init; }
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}