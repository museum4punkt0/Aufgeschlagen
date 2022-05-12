using UnityEditor;
using System.IO;
using UnityEngine;
using System.Diagnostics;
using System;
using System.Threading;

public class UpdateAppModels: ScriptableObject
{
    private readonly static string SOURCE_FOLDER_PREF_KEY = "PREF_KEY_SOURCE_FOLDER";
    private readonly static string DESTINATION_FOLDER_PREF_KEY = "PREF_KEY_DESTINATION_FOLDER";

    [MenuItem("MicroMovie/Model Generator/Config App Model Gen/Select app models source solder")]
    static void SelectSourceFolder()
    {
        string path = EditorUtility.OpenFolderPanel("Select Modelgenerator source folder", "", "");
        PlayerPrefs.SetString(SOURCE_FOLDER_PREF_KEY, path);
        UnityEngine.Debug.Log("SelectSourceFolder " + path);
    }

    [MenuItem("MicroMovie/Model Generator/Config App Model Gen/Select app models destination folder in project")]
    static void SelectDestinationFolder()
    {
        string path = EditorUtility.OpenFolderPanel("select modelgenerator destination folder", "", "");
        PlayerPrefs.SetString(DESTINATION_FOLDER_PREF_KEY, path);
        UnityEngine.Debug.Log("SelectDestinationFolder " + path);
    }

    private static string findPHPPath()
    {
        string path;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            path = ExecuteBashCommand("where php.exe");
        }
        else
        {
            path = "/usr/local/bin/php";
        }
        UnityEngine.Debug.Log("pathToPHP:" + path);
        return path;
    }


    [MenuItem("MicroMovie/Model Generator/Generate App Models")]
    static void PerformCoreModelGenerator()
    {
        // lets say we want to run this command:    
        //  t=$(echo 'this is a test'); echo "$t" | grep -o 'is a'
        //var output = ExecuteBashCommand("t=$(echo 'this is a test'); echo \"$t\" | grep -o 'is a'");
        // ../submodules/unity_guidepilotcore/Assets/Scripts/Model
        string sourceFolder = PlayerPrefs.GetString(SOURCE_FOLDER_PREF_KEY);
        if (string.IsNullOrEmpty(sourceFolder))
        {
            UnityEngine.Debug.LogError("SourceFolder not configured");
            return;
        }

        string destinationFolder = PlayerPrefs.GetString(DESTINATION_FOLDER_PREF_KEY);
        if (string.IsNullOrEmpty(destinationFolder))
        {
            UnityEngine.Debug.LogError("destination folder not configured");
            return;
        }

        string pathToPHP = findPHPPath();
        if (string.IsNullOrEmpty(pathToPHP) || pathToPHP.Contains("not found"))
        {
            UnityEngine.Debug.LogError("PHP is not installed or could not be found");
            return;
        }

        UnityEngine.Debug.Log("sourceFolder:" + sourceFolder);
        UnityEngine.Debug.Log("destinationFolder:" + destinationFolder);
        UnityEngine.Debug.Log("pathToPHP:" + pathToPHP);
        var output = ExecuteBashCommand(pathToPHP + " ../submodules/unity_guidepilotcore/submodules/php_GPEnumGenerator/Source/Main.php --sourceFolder '" + sourceFolder + "' --generators 'unity swift kotlin'");

        // output the result
        //Console.WriteLine(output);
        UnityEngine.Debug.Log("out: " + output);

        Directory.Delete(destinationFolder, true);
        DirectoryCopy(sourceFolder + "/Output/Unity", destinationFolder, true);

    }

    static string ExecuteBashCommand(string command)
    {
        UnityEngine.Debug.Log("ExecuteBashCommand: " + command);
        // according to: https://stackoverflow.com/a/15262019/637142
        // thans to this we will pass everything as one command
        string fileName;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            fileName = "cmd.exe";
        }
        else
        {
            fileName = "/bin/zsh";
        }

        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = "-c \"" + command + "\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };

        proc.Start();
        string result = proc.StandardOutput.ReadToEnd();
        proc.WaitForExit();

        return result;
    }

    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();

        // If the destination directory doesn't exist, create it.       
        Directory.CreateDirectory(destDirName);

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string tempPath = Path.Combine(destDirName, file.Name);
            file.CopyTo(tempPath, false);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
            }
        }
    }

    public static void RecursiveDelete(DirectoryInfo baseDir)
    {
        if (!baseDir.Exists)
            return;

        foreach (var dir in baseDir.EnumerateDirectories())
        {
            RecursiveDelete(dir);
        }
        baseDir.Delete(true);
    }
}
