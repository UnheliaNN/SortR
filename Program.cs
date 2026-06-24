global using System;
using System.Runtime.CompilerServices;

namespace SortR;
public static class Program
{
    public static void Main()
    {
        if (!Directory.Exists(RuntimeData.AppDirPath))
        {
            Register.Log("Created app directory", 'I');
            Directory.CreateDirectory(RuntimeData.AppDirPath);
        }
        Config.Load(); // Loading configs from json file

        foreach (Job job in Config.Instance.Jobs) // Collecting all Jobs from configs
        {
            try // Creating target directory
            {
                if (!Directory.Exists(job.Path))
                    Directory.CreateDirectory(job.Path);
            }
            catch (Exception ex)
            {
                Register.Log($"Job {job.Category} is skipped due to the exception: {ex.Message}", 'E'); // Skipping Job if directory couldn't be created
                continue;
            }

            int filesAmount = 0;
            foreach (string name in job.Names) // Collecting all files that match name parameter and moving them
            {
                string[] targetFiles = [.. Directory.EnumerateFiles(RuntimeData.Buffer, $"*{name}*", SearchOption.AllDirectories)];
                filesAmount += MoveFiles(targetFiles, job);
            }
            foreach (string extension in job.Extensions) // Collecting all files that match extension parameter and moving them
            {
                string[] targetFiles = [.. Directory.EnumerateFiles(RuntimeData.Buffer, $"*{extension}", SearchOption.AllDirectories)];
                filesAmount += MoveFiles(targetFiles, job);
            }
            if (filesAmount > 0)
                Register.Log($"Moved {filesAmount} files to category: {job.Category}", 'I');
        }

        Config.Save(); // Saving configs
    }
    public static int MoveFiles(string[] targetFiles, Job Job)
    {
        int filesAmount = 0;

        foreach (string file in targetFiles)
        {
            string targetFileName = Path.Combine(Job.Path, Path.GetFileName(file));
            
            try
            {
                if (File.Exists(targetFileName)) // Options to do with existing file
                {
                    switch (Config.Instance.Settings.ExistingFilesAction.ToLower()) //TODO MAKE REPLACE OPTION
                    {
                        case "replace":
                            File.Delete(targetFileName);
                            File.Move(file, targetFileName);
                            Register.Log($"Replaced {file}: File already exist", 'I');
                            filesAmount++;
                            break;
                        case "ignore":
                            Register.Log($"Ignored file {file}: File already exist", 'I');
                            break;
                        default:
                            Register.Log($"Couldn't find correct option for ExistingFilesAction, make sure it is ignore or replace. Ignored existing file {file} by default", 'W');
                            break;
                    }
                    continue;
                }
                File.Move(file, targetFileName);
                filesAmount++;
            }
            catch (Exception ex)
            {
                Register.Log($"Couldn't move file {Path.GetFileName(file)}: {ex.Message}", 'E');
            }
        }
        return filesAmount;
    }
}
public static class Register
{
    public static string? Logfile {get; set;}
    /// <summary>
    /// Writes message to the console and logfile. Adds name of caller member and time mark
    /// </summary>
    /// <param name="message">Message to be displayed</param>
    /// <param name="type">Type of message - I,W,E for Info, Warning and Error. Unknown type is default</param>
    /// <param name="caller">Automaticly writes name of caller method, but can be defined manually</param>
    public static void Log(string message, char type, [CallerMemberName] string caller = "")
    {
        string Type = type switch // Changing type for console color and message
        {
            'I' => "Info",
            'W' => "Warning",
            'E' => "Error",
            _ => "Unknown", // Default type is unknown
        };
        string Message = $"[{DateTime.Now:HH:mm:ss}][{caller}][{Type}]: {message}";

        switch (type)
        {
            case 'W': // Yellow color for warnings
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case 'E': // Red color for errors
                Console.ForegroundColor = ConsoleColor.Red;
                break;
        }
        Console.WriteLine(Message); // Writes message to console
        Console.ResetColor(); // Resetting color

        if (Config.Instance.Settings.WriteLogs)
        {
            if (!File.Exists(Logfile))
                Logfile = Path.Combine(Config.Instance.Settings.LogsDirectory, DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss") + ".log");
            File.AppendAllText(Logfile, Message); // Writes message to file
        }
    }
    /// <summary>
    /// Writes message to the console and logfile.
    /// </summary>
    /// <param name="message">Message to be displayed</param>
    public static void Log(string message)
    {
        if (Config.Instance.Settings.WriteLogs)
        {
            if (!File.Exists(Logfile))
                Logfile = Path.Combine(Config.Instance.Settings.LogsDirectory, DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss") + ".log");
            File.AppendAllText(Logfile, message);
        }
        Console.WriteLine(message);
    }
}