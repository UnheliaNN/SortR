using System.Runtime.InteropServices;

namespace SortR;

static class RuntimeData
{
    private static string? _appDirpath;
    public static string AppDirPath 
    {
        get
        {
            if (Directory.Exists(_appDirpath))
                return _appDirpath;
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return _appDirpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SortR");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return _appDirpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "SortR");
            else
                throw new SystemException("Os is unsupported");
        }
    }

    private static string? _jsonpath;
    public static string JsonPath 
    {
        get
        {
            if (Directory.Exists(_jsonpath))
                return _jsonpath;
            return _jsonpath = Path.Combine(AppDirPath, "config.json");
        }
    }

    public static string Buffer 
    {
        get
        {
            if (Directory.Exists(Config.Instance.Settings.BufferPath))
                return Config.Instance.Settings.BufferPath;
            else
                Directory.CreateDirectory(Config.Instance.Settings.BufferPath);
                return Config.Instance.Settings.BufferPath;
        } 
    }
}