using System;
public class TestAssetLocations
{
    public static string Assets = "../Export/Assets/";

    public static string OSXFolder = "/MacOS";
    public static string IOSFolder = "/iOS";
    public static string AndroidFolder = "/Android";
    public static string OtherFolder = "/Other";

    public static string GetPlatformAssetFolder()
    {
#if UNITY_EDITOR_OSX
        return TestAssetLocations.Assets + TestAssetLocations.OSXFolder;
#elif UNITY_EDITOR
        return TestAssetLocations.Assets + TestAssetLocations.OtherFolder;
#else
    return null;
#endif
    }
}
