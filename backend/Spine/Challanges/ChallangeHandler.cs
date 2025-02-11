namespace OnlyBans.Backend.Spine.Challanges;

public class ChallangeHandler
{
    private int handlerID;
    public ChallangeHandler()
    {
        handlerID = HandlerTracker.lChallangeHandlers.Count;
        HandlerTracker.lChallangeHandlers.Add(this);
    }
    
}