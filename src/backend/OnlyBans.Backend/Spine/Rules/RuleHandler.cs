﻿namespace OnlyBans.Backend.Spine.Rules;

public class RuleHandler
{
    private int handlerID;
    public RuleHandler()
    {
        handlerID = HandlerTracker.lRuleHandlers.Count;
        HandlerTracker.lRuleHandlers.Add(this);
    }

    public static bool checkIfUserIsBanned(Guid userId)
    {
        
    }
}