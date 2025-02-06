namespace OnlyBans.Backend.Spine.Rules;

public class RuleHandler
{
    private int handlerID;
    public RuleHandler()
    {
        handlerID = HandlerTracker.lRuleHandlers.Count;
        HandlerTracker.lRuleHandlers.Add(this);
    }
    
    public bool checkTitle(string title)
    {
        if (title.Length > 42)
            return false;
        return true;
    }
}