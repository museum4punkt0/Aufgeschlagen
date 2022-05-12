using UnityEditor;
using System.IO;
using UnityEngine;
using System.Diagnostics;
using System;

public class UpdateCoreModels
{
    [MenuItem("MicroMovie/Model Generator/Generate Core Models")]
    static void PerformCoreModelGenerator()
    {
        // lets say we want to run this command:    
        //  t=$(echo 'this is a test'); echo "$t" | grep -o 'is a'
        //var output = ExecuteBashCommand("t=$(echo 'this is a test'); echo \"$t\" | grep -o 'is a'");
        var output = ExecuteBashCommand("php ../submodules/unity_guidepilotcore/submodules/php_GPEnumGenerator/Source/Main.php; cp -R ../submodules/unity_guidepilotcore/submodules/php_GPEnumGenerator/Output/Unity/* ../submodules/unity_guidepilotcore/Assets/Scripts/Model");

        // output the result
        //Console.WriteLine(output);
        UnityEngine.Debug.Log("out: " + output);
    }

    static string ExecuteBashCommand(string command)
    {
        // according to: https://stackoverflow.com/a/15262019/637142
        // thans to this we will pass everything as one command
        command = command.Replace("\"", "\"\"");

        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = "-c \"" + command + "\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };

        proc.Start();
        proc.WaitForExit();

        return proc.StandardOutput.ReadToEnd();
    }
}
