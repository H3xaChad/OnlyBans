namespace OnlyBans.Backend.Models.Rules;

public class RuleTextDto
{
    public string Text { get; }

    public RuleTextDto(Rule rule)
    {
        Text = rule.Text;
    }
}