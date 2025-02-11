using System.Net;
using DotNetEnv;

namespace OnlyBans.Backend.Spine.AI
{
    public class AiApiKeyHandler
    {
        private readonly string _filePath;

        public AiApiKeyHandler()
        {
            Env.Load("secrets.env");
            _filePath = Env.GetString("API_KEY_PATH");
            Console.WriteLine(_filePath);
        }

        public string GetApiKey()
        {
            if (!File.Exists(_filePath))
            {
                //logger einbauen!
                throw new Exception($"API key file not found: {_filePath}");
            }
            return File.ReadAllText(_filePath); 
        }
    }
}