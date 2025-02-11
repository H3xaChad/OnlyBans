using OnlyBans.Backend.Database;
using OnlyBans.Backend.Models.Posts;
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
        return _context.Rules
            .Where(r => r.RuleCategory == RuleEnum.contentRule)
            .Select(r => r.Text)
            .ToList();
    }

    private List<string> getTitleRules()
    {
        return _context.Rules
            .Where(r => r.RuleCategory == RuleEnum.titleRule)
            .Select(r => r.Text)
            .ToList();
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