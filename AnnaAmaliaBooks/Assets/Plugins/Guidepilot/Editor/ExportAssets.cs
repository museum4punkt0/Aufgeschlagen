using UnityEditor;
using System.IO;
using UnityEngine;

public class ExportAssets
{
    [MenuItem("MicroMovie/Export Assets/MacOS")]
    static void PerformMacOSAssetsBuild()
    {
        PerformAssetBuild(BuildTarget.StandaloneOSX);
    }

    [MenuItem("MicroMovie/Export Assets/Windows")]
    static void PerformWindowsAssetsBuild()
    {
        PerformAssetBuild(BuildTarget.StandaloneWindows);
    }

    [MenuItem("MicroMovie/Export Assets/iOS")]
    static void PerformiOSAssetsBuild()
    {
        PerformAssetBuild(BuildTarget.iOS);
    }

    [MenuItem("MicroMovie/Export Assets/Android")]
    static void PerformAndroidAssetsBuild()
    {
        PerformAssetBuild(BuildTarget.Android);
    }

    static void PerformAssetBuild(BuildTarget target)
    {
        string assetsExportFolder = TestAssetLocations.Assets;

        switch (target)
        {
            case BuildTarget.StandaloneOSX:
                assetsExportFolder = assetsExportFolder + TestAssetLocations.OSXFolder;
                break;

            case BuildTarget.iOS:
                assetsExportFolder = assetsExportFolder + TestAssetLocations.IOSFolder;
                break;

            case BuildTarget.Android:
                assetsExportFolder = assetsExportFolder + TestAssetLocations.AndroidFolder;
                break;

            default:
                assetsExportFolder = assetsExportFolder + TestAssetLocations.OtherFolder;
                break;

        }

        try
        {
            if (!Directory.Exists(assetsExportFolder))
            {
                Directory.CreateDirectory(assetsExportFolder);
            }
        }
        catch (IOException ex)
        {
            Debug.Log(ex.Message);
        }

        BuildPipeline.BuildAssetBundles(assetsExportFolder, BuildAssetBundleOptions.ChunkBasedCompression, target);
    }

}
