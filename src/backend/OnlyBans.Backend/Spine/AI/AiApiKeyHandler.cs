using DotNetEnv;

namespace OnlyBans.Backend.Spine.AI
{
    public class AiApiKeyHandler
    {
        private readonly string _filePath;

        public AiApiKeyHandler()
        {
            Env.Load("spine/secret.env");
            _filePath = Env.GetString("API_KEY_PATH");
            Console.Write(_filePath);
        }

        public string GetApiKey()
        {
            if (!File.Exists(_filePath))
            {
                //logger einbauen!
                //throw new FileNotFoundException("API key file not found.", _filePath);
            }

            return File.ReadAllText(_filePath).Trim();
        }
    }
}