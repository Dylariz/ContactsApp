using Newtonsoft.Json;

namespace ContactsApp.Data;

// ReSharper disable once ClassNeverInstantiated.Global
public class AppConfig
{
    private static readonly string ConfigPath = Path.Combine("Data", "appconfig.json");
    
    private static AppConfig? _instance;
    private static object _sync = new();
    
    public required IReadOnlyList<string> Genders { get; init; }
    
    public static AppConfig GetInstance()
    {
        if (_instance == null)
        {
            lock (_sync)
            {
                if (_instance == null)
                {
                    var reader = new JsonTextReader(new StreamReader(ConfigPath));
                    var serializer = new JsonSerializer();
                    _instance = serializer.Deserialize<AppConfig>(reader) ?? throw new ArgumentException("Invalid configuration file");
                }
            }
        }
        return _instance;
    }
}