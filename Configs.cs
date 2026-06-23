using System.Text.Json;
using System.Text.Json.Serialization;

namespace SortR;

public static class Config
{
    public static AppConfig Instance { get; private set; } = new();

    private static readonly JsonSerializerOptions options =  new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    /// <summary>
    /// Fetches configs from json file and loads it to the program
    /// </summary>
    public static void Load()
    {
        try
        {
            try
            {
                AppConfig? json = JsonSerializer.Deserialize<AppConfig>(File.ReadAllText(RuntimeData.JsonPath), options);
                #pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (!json.Settings.ResetConf)
                {
                    Instance = json;
                    Register.Log("Successfully imported configs to runtime", 'I');
                }
                else
                {
                    Register.Log("Configs were reseted, using default", 'I');
                }
                #pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            catch(FileNotFoundException)
            {
                Register.Log("Couldn't find config file. Creating new", 'I');
            }
        }
        catch (Exception ex)
        {
           Register.Log($"Json deserialization failed: {ex.Message}, using default configs", 'W');
        }
    }
    /// <summary>
    /// Saves current config to the json file
    /// </summary>
    public static void Save()
    {
        try
        {
            File.WriteAllText(RuntimeData.JsonPath ,JsonSerializer.Serialize(Instance, options));
            Register.Log("Json serialization succsessful, configs written to the file", 'I');
        }
        catch
        {
            Register.Log("Json serialization failed, configs wasn't saved", 'W');
        }
    }
}
public class AppConfig
{
    [JsonPropertyName("appName")]
    public string AppName { get; } = "SortR";
    [JsonPropertyName("appVersion")]
    public string AppVersion { get; } = "1.0";
    [JsonPropertyName("settings")]
    public Settings Settings { get; set; } = new();
    [JsonPropertyName("works")]
    public List<Work> Works { get; set;} =
    [
        new Work
        { 
            Category = "Images",
            Path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), 
            Extensions = [".png", ".jpg", ".webp", ".gif", ".bmp", ".heic", ".psd", ".ai", ".svg", ".vsdx"]
        },
        new Work
        {
            Category = "Documents",
            Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
            Extensions = [".docx", ".pdf", ".txt", ".odt", ".md", ".rtf", ".tex"]
        },
        new Work
        {
            Category = "Music",
            Path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), 
            Extensions = [".mp3", ".aif", ".ogg", ".wav", ".wma"]
        },
        new Work
        { 
            Category = "Programs",
            Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Programs"), 
            Extensions = [".exe", ".app", ".msi", ".apk", ".bat", ".bin", ".jar", ".ipa", ".sh", ".run"]
        },
        new Work
        { 
            Category = "Videos",
            Path = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), 
            Extensions = [".mp4", ".mpg", ".flv", ".mov", ".avi", ".wmv"]
        },
        new Work
        { 
            Category = "3DModels",
            Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "3DModels"), 
            Extensions = [".blend", ".rdm", ".rds", ".dae", ".fbx", ".max", ".obj"]
        }
    ];
}
public class Work
{
    [JsonPropertyName("category")]
    public string Category { get; set; } = "";
    [JsonPropertyName("path")]
    public string Path { get; set; } = "";
    [JsonPropertyName("names")]
    public string[] Names { get; set; } = [];
    [JsonPropertyName("extensions")]
    public string[] Extensions { get; set; } = [];
}
public class Settings
{
    [JsonPropertyName("bufferName")]
    public string BufferPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Buffer");
    [JsonPropertyName("logsDirectory")]
    public string LogsDirectory { get; set; } = Path.Combine(RuntimeData.AppDirPath, "Logs");
    [JsonPropertyName("existingFilesAction")]
    public string ExistingFilesAction { get; set; } = "ignore";
    [JsonPropertyName("writeLogs")]
    public bool WriteLogs { get; set; } = false;
    [JsonPropertyName("resetConfig")]
    public bool ResetConf { get; set; } = true; //TODO CHANGE TO FALSE FOR PROD
}