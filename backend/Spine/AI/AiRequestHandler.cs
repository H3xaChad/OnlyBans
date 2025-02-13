using Microsoft.Identity.Client;

namespace OnlyBans.Backend.Spine.AI;

using OpenAI;
using OpenAI.Chat;
using System;
using System.Threading.Tasks;
using System.Text.Json;

public class AiRequestHandler
{
    private int handlerId;
    private readonly string _filePath;
    private AiApiKeyHandler aa;
    private ChatClient client;
    
    public AiRequestHandler()
    {
        handlerId = HandlerTracker.lAiRequestHandlers.Count;
        HandlerTracker.lAiRequestHandlers.Add(this);
        aa = new AiApiKeyHandler();
        client = new(model: "gpt-4o", apiKey: GetApiKey());
    }
    
    public string GetApiKey()
    {
        return aa.GetApiKey();
    }
    
    private List<bool> results;

    //--------------------------------------------------------------------------------------------------------------------------
    //title prompt
    private static readonly string promptIntroTitle = "Du bist ein Validierer und sollst einen Titel gegen " +
                                                      "(mehrere) Regeln validieren. Deine Antwort muss eine Liste sein die " +
                                                      "folgendermaßen aussehen muss: ";

    private static string titlePrompt = $"";

    //--------------------------------------------------------------------------------------------------------------------------
    //content prompt
    private static readonly string promptIntroContent = "Du bist ein Validierer und sollst den Content eines Posts gegen " +
                                                        "(mehrere) Regeln validieren. Deine Antwort muss eine liste sein die " +
                                                        "folgendermaßen aussehen muss: ";
    
    private static string contentPrompt = $"";
    
    //--------------------------------------------------------------------------------------------------------------------------
    //general prompt
    private static readonly string promptResultList = $"[*hier die bool werte für jede regel. true oder false, " +
                                                      $"je kofirmität des zu validierenden Textx gegenüber aller regeln*]";

    private static readonly string promptReturnExplanation = $"Diese Liste darf ausschließlich true oder false werte " +
                                                             $"enthalten die in der selben reihenfolge angeordnet sind, " +
                                                             $"wie die Regeln gegen die Validiert wird!\n" +
                                                             $"Hier sind die Regeln:\n";

    private static readonly string promptTextToValidate = $"Hier ist der Text der gegen die Regeln validiert werden soll:\n";
    //--------------------------------------------------------------------------------------------------------------------------
    public bool validateRule(string text, List<string> rules, int category)
    {
        results = new List<bool>();

        if (category == 0) {
            titlePrompt = $"{promptIntroTitle}\n{promptResultList}\n" +
                          $"{promptReturnExplanation}\n{getRulesAsString(rules)}\n" +
                          $"{promptTextToValidate}\n{text}";
            results = getAnswers(rules, titlePrompt, text);
        }
        else if (category == 1) {
            contentPrompt = $"{promptIntroContent}\n{promptResultList}\n" +
                            $"{promptReturnExplanation}\n{getRulesAsString(rules)}\n" +
                            $"{promptTextToValidate}\n{text}";
            results = getAnswers(rules, contentPrompt, text);
        }
        else {
            throw new Exception("Invalid category");
        }
        if (results.Count != rules.Count) {
            throw new Exception("Invalid number of results");
        }
        if (results.Contains(false)) {
            return false;
        }
        return true;
    }

    private string getRulesAsString(List<string> rules) {
        string s = "";
        
        for (int i = 0; i < rules.Count; i++)
        {
            s += $"Regel {i + 1}: {rules[i]}, ";
        }
        //Console.WriteLine($"list: {s}");
        return s;
    }

    private List<bool> getAnswers(List<string> rules, string prompt, string textToValidate) {
        ChatCompletion completionNew;
        
        completionNew = pingAI(prompt);
        Console.WriteLine($"###############\nCompletion:\n{completionNew.Content[0].Text}\n");

        completionNew = pingAI($"Bist du sicher? Rules: {getRulesAsString(rules)}; Text zum validieren: {textToValidate}; Wichtig: du darfst immer noch ausschließlich so antworten: {promptResultList} ");
        Console.WriteLine($"###############\nCompletion:\n{completionNew.Content[0].Text}\n");
        string validationResult = completionNew.Content[0].Text.ToLower();
        List<bool> boolList = JsonSerializer.Deserialize<List<bool>>(validationResult);
        Console.WriteLine(boolList);
        return boolList;
    }

    private ChatCompletion pingAI(string prompt)
    {
        ChatCompletion comp;
        try { 
            comp = client.CompleteChat(prompt);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw new Exception("Validation result never returned");
        }

        return comp;
    }
}