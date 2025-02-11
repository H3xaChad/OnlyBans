namespace OnlyBans.Backend.Spine.AI;

using OpenAI;
using OpenAI.Chat;
using System;
using System.Threading.Tasks;

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
    
    /*
    private List<bool> getAnswer(List<string> rules)
    {
        List<bool> results = new List<bool>();
        var client = new OpenAIClient(GetApiKey());
        var prompt = "Du bist ein Validierer und sollst einen Titel gegen mehrere Regeln validieren. Deine Antwort muss im JSON format sein und muss folgendermaßen aussehen: resultBools:[*hier die bool werte für jede regel. true oder false, je kofirmität des titels gegenüber aller regeln*],";
        var completion = client.Completions.CreateCompletion(
            new CreateCompletionRequest
            {
                Model = Model.Davinci,
                MaxTokens = 100,
                Prompt = prompt,
                Temperature = 0.7,
                Stop = new string[] { "\n" }
            }
        );
        return results;
    }
    */
    
    private List<bool> results;

    /*
    private static string titleIntro = "Du bist ein Validierer und sollst einen Titel gegen mehrere Regeln validieren. Deine Antwort muss im JSON format sein und muss folgendermaßen aussehen: ";
    private static string titleKeyOne = $"resultBools:[*hier die bool werte für jede regel. true oder false, je kofirmität des titels gegenüber aller regeln*],";
    private static string titleKeyTwo = $"";
    private string titlePrompt = $"{titleIntro}{titleKeyOne}{titleKeyTwo}";
    */
    public bool validateRule(List<string> rules, int category)
    {
        results = new List<bool>();
        //results = getAnswer(rules);
        
        
        /*for (int i = 0; i < rules.Count; i++)
        {
            if (rules[i] == "title")
            {
                results.Add();
            }
        }*/
        
        ChatClient client = new(model: "gpt-4o", apiKey: GetApiKey());
        ChatCompletion completion = client.CompleteChat("Say 'this is a test.'");
        Console.WriteLine("--------------------------------");
        Console.WriteLine(completion);
        Console.WriteLine("--------------------------------");
        
        return false;
    }
}