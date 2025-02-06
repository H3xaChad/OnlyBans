using OnlyBans.Backend.Spine.Validation;
using OnlyBans.Backend.Spine.Challanges;
using OnlyBans.Backend.Spine.Rules;

namespace OnlyBans.Backend.Spine;

public static class HandlerTracker
{
    public static List<ValidationHandler> lValidationHandlers { get; set; } = new();
    public static List<ChallangeHandler> lChallangeHandlers { get; set; } = new();
    public static List<RuleHandler> lRuleHandlers { get; set; } = new();
    
}