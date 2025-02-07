using OnlyBans.Backend.Spine.AI;
using OpenAI;

namespace OnlyBans.Backend.Spine.Rules;

public class RuleHandler
{
    private int handlerID;
    private AiRequestHandler ar;
    public RuleHandler()
    {
        handlerID = HandlerTracker.lRuleHandlers.Count;
        HandlerTracker.lRuleHandlers.Add(this);
        ar = new AiRequestHandler();
        
    }
    
    public bool checkTitle(string title)
    {
        ar.GetApiKey();
        return false;
    }
}