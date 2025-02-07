namespace OnlyBans.Backend.Spine.AI;

public class AiRequestHandler
{
    private int handlerId;
    private readonly string _filePath;
    private AiApiKeyHandler aa;
    
    public AiRequestHandler()
    {
        handlerId = HandlerTracker.lAiRequestHandlers.Count;
        HandlerTracker.lAiRequestHandlers.Add(this);
        aa = new AiApiKeyHandler();
    }
    
    public string GetApiKey()
    {
        return aa.GetApiKey();
    }
}