using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MMBuilderScripts
{
    public static void BuildWebGLBundles()
    {
        string projectRootPath = Path.GetFullPath(Application.dataPath + @"\..\");
        string buildsPath = projectRootPath + @"Builds\AssetBundles\scene\";
        ExecuteAssetBundleBuild(BuildTarget.WebGL, buildsPath);
    }

    private static void ExecuteAssetBundleBuild(BuildTarget targetPlatform, string buildsPath)
    {
        // ***NOTE: Arg8 = game version, Arg9 = asset bundle version, Arg10 = environment (development/production)
        string builderVersion;
        string bundlesVersion;

        try
        {
            var args = Environment.GetCommandLineArgs();

            builderVersion = args[8];
            bundlesVersion = args[9];


            Debug.Log("---Start rebuilding asset bundles...");
            Directory.CreateDirectory(buildsPath);
            BuildPipeline.BuildAssetBundles(buildsPath, BuildAssetBundleOptions.None, targetPlatform);
            Debug.Log("---Building asset bundles is done! Location: " + buildsPath);


            Debug.Log("---All tasks done. Exiting...");
            EditorApplication.Exit(0);
        }
        catch (Exception e)
        {
            Debug.Log($"---Error building Asset Bundles {e}, exiting...");
            EditorApplication.Exit(1);
        }
    }
}
