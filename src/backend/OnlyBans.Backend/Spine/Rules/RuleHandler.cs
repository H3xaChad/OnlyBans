using OnlyBans.Backend.Database;
using OnlyBans.Backend.Spine.AI;
using OpenAI;

namespace OnlyBans.Backend.Spine.Rules;

public class RuleHandler
{
    private AppDbContext _context;
    private int handlerID;
    private AiRequestHandler ar;
    private List<string> titleRules;
    private List<string> contentRules;
    public RuleHandler(AppDbContext _context)
    {
        this._context = _context;
        handlerID = HandlerTracker.lRuleHandlers.Count;
        HandlerTracker.lRuleHandlers.Add(this);
        ar = new AiRequestHandler();
        getRules();
    }

    public void getRules()
    {
        titleRules = getTitleRules();
        contentRules = getContentRules();
    }

    private List<string> getContentRules()
    {
        throw new NotImplementedException();
    }

    private List<string> getTitleRules()
    {
        throw new NotImplementedException();
    }


    public bool checkTitle(string title)
    {
        if (ar.validateRule(titleRules, 0))
        {
            return true;
        }
        return false;
    }
}