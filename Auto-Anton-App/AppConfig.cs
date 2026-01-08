using System;
using System.IO;
using System.Text.Json;

namespace Auto_Anton_App
{
    public class AppConfig
    {
        public string OllamaPath { get => ollamaPath; set => ollamaPath = value; }
        private string ollamaPath = string.Empty;

        static string ConfigDir =>
            Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData),
                "Auto-Anton");

        static string ConfigFile => Path.Combine(ConfigDir, "AppConfig.json");

        public static AppConfig Load()
        {            
            try
            {
                if (File.Exists(ConfigFile))
                {
                    return JsonSerializer.Deserialize<AppConfig>(
                        File.ReadAllText(ConfigFile));
                }
            }
            catch { }

            return new AppConfig();
        }

        public void Save()
        {
            Directory.CreateDirectory(ConfigDir);
            File.WriteAllText(ConfigFile,
                JsonSerializer.Serialize(this, new JsonSerializerOptions
                {
                    WriteIndented = true
                }));
        }
    }
}
