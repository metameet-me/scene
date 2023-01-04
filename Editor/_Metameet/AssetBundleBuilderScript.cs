using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO.Compression;
using UnityEngine.Networking;
using static LoginDialog;

public class AssetBundleBuilderScript : MonoBehaviour
{

    [MenuItem("Metameet/Configuration")]
    public static void Login()
    {
        LoginDialog.Init();
    }
    [MenuItem("Metameet/ValidateScene")]
    public static void ValidateScene()
    {
        SceneValidation.ValidateScene();
    }

    [MenuItem("Metameet/RemoveDeletedScenes")]
    public static void CleanUpDeletedScenes()
    {
        var currentScenes = EditorBuildSettings.scenes;
        var filteredScenes = currentScenes.Where(ebss => File.Exists(ebss.path)).ToArray();
        EditorBuildSettings.scenes = filteredScenes;
    }

    [MenuItem("Metameet/Upload/All")]
    public static void CreateAssetsAndUploadAll()
    {
        CreateAndUpload(BuildTarget.WebGL);
        CreateAndUpload(BuildTarget.StandaloneWindows);
        CreateAndUpload(BuildTarget.Android);
        CreateAndUpload(BuildTarget.iOS);
    }
    [MenuItem("Metameet/Upload/Webgl")]
    public static  void CreateAssetsAndUploadWebGL()
    {
        CreateAndUpload(BuildTarget.WebGL);
    }

    [MenuItem("Metameet/Upload/StandaloneWindows")]
    public static void CreateAssetsAndUploadStandalone()
    {
        CreateAndUpload(BuildTarget.StandaloneWindows);
    }

    [MenuItem("Metameet/Upload/Android")]
    public static void CreateAssetsAndUploadAndroid()
    {
        CreateAndUpload(BuildTarget.Android);
    }

    [MenuItem("Metameet/Upload/IOS")]
    public static void CreateAssetsAndUploadIOS()
    {
        CreateAndUpload(BuildTarget.iOS);
    }

  

    public static void CreateAndUpload(BuildTarget platform)
    {

        if (!SceneValidation.ValidateScene())
        {
            EditorUtility.DisplayDialog("Invalid Scene", "Check the console log for details", "Ok");
        }

        if (MetameetConfig.token?.access_token == null)
        {
            EditorUtility.DisplayDialog("Invalid Login", "Must login successfully before completing this task ", "Ok");
            LoginDialog.Init();
            return;
        }

        if (string.IsNullOrEmpty(MetameetConfig.BaseURLWebService))
        {
            EditorUtility.DisplayDialog("Please select an environment", "Environment missing ", "Ok"); 
            LoginDialog.Init();
            return;
        }

        if (string.IsNullOrEmpty(MetameetConfig.EventID))
        {
            EditorUtility.DisplayDialog("Please enter an world id", "World id missing", "Ok");
            LoginDialog.Init();
            return;
        }

        if (Directory.Exists("AssetBundles/scene"))
            Directory.Delete("AssetBundles/scene", true);
        if (Directory.Exists("AssetBundles/Output"))
            Directory.Delete("AssetBundles/Output", true);
        if (!Directory.Exists("AssetBundles/scene"))
        {
            Directory.CreateDirectory("AssetBundles");
            Directory.CreateDirectory("AssetBundles/scene");
        }
        if (!Directory.Exists("AssetBundles/Output"))
            Directory.CreateDirectory("AssetBundles/Output");
        Scene scene = SceneManager.GetActiveScene();
        string localPath = "AssetBundles/scene";
        string path = "AssetBundles/Output/Output.zip";




        UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        BuildPipeline.BuildAssetBundles(localPath, BuildAssetBundleOptions.None, BuildTarget.WebGL); 

        ZipFile.CreateFromDirectory(localPath, path, System.IO.Compression.CompressionLevel.Optimal, false);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        var fileData = File.ReadAllBytes(path);
        formData.Add(new MultipartFormFileSection(scene.name, fileData));

        UnityWebRequest request;
     
        request = UnityWebRequest.Post(MetameetConfig.BaseURLWebService + "Scene/Upload?eventid=" + MetameetConfig.EventID +"&platform=" + platform.ToString(), formData);
        request.SetRequestHeader("Authorization", "bearer " + MetameetConfig.token.access_token);
        //  request.SetRequestHeader("Authorization", "bearer " + Token);
        request.SendWebRequest();

        while (!request.isDone)
        {
      
        }

        if(request.isNetworkError)
        {
            EditorUtility.DisplayDialog("Error when uploading",        "Check your file size. " + request.error, "Ok");
        }
        else if(!request.isNetworkError)
        {
            EditorUtility.DisplayDialog("Scene Uploaded", "Join your world to test" + request.error, "Ok");
        }



        if (Directory.Exists("AssetBundles/Output"))
            Directory.Delete("AssetBundles/Output", true);
    }

    //[MenuItem("Metameet/Shader/Fix pink shaders")]
    //public static void FixPinkShaders()
    //{
    //    GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
    //    foreach (GameObject go in allObjects)
    //    {
    //        if (go.GetComponent<Renderer>() != null)
    //        {
    //            Material[] mats = go.GetComponent<Renderer>().materials;
    //            foreach (Material mat in mats)
    //            {
    //                mat.shader = Shader.Find("Universal Render Pipeline/Lit");
    //            }
    //        }
    //    }

    //}

    public static void BuildiOSBundles()
    {
        string projectRootPath = Path.GetFullPath(Application.dataPath + @"\..\");
        string buildsPath = projectRootPath + @"Builds\AssetBundles\iOS\CUSTOM\";
        ExecuteAssetBundleBuild(BuildTarget.iOS, buildsPath);
    }

    public static void BuildWebGLBundles()
    {
        string projectRootPath = Path.GetFullPath(Application.dataPath + @"\..\");
        string buildsPath = projectRootPath + @"Builds\AssetBundles\WebGL\CUSTOM\";
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
            string assetBundlesOutputPath = $"{buildsPath}V{builderVersion}.{bundlesVersion}\\";
            Directory.CreateDirectory(assetBundlesOutputPath);
            BuildPipeline.BuildAssetBundles(assetBundlesOutputPath, BuildAssetBundleOptions.None, targetPlatform);
            Debug.Log("---Building asset bundles is done! Location: " + assetBundlesOutputPath);


            Debug.Log("---All tasks done. Exiting...");
            EditorApplication.Exit(0);
        }
        catch (Exception e)
        {
            Debug.Log($"---Error building Asset Bundles {e}, exiting...");
            EditorApplication.Exit(1);
        }
    }

    [MenuItem("Assets/Set Asset Bundle Names In Children")]
    public static void SetAssetBundleNames()
    {
        var selection = Selection.objects.ToList();
        foreach (var item in selection)
        {
            Debug.Log(AssetDatabase.GetAssetPath(item));
            SetAssetBundleNamesInFolder(item);
        }
    }

    public static void SetAssetBundleNamesInFolder(UnityEngine.Object obj)
    {
        string assetBundleName = AssetDatabase.GetAssetPath(obj).Split('/')[AssetDatabase.GetAssetPath(obj).Split('/').Length - 1];

        var prefabs = AssetDatabase.FindAssets("t:prefab", new string[] { AssetDatabase.GetAssetPath(obj) });
        foreach(var item in prefabs)
        {
            //Debug.Log(AssetDatabase.GUIDToAssetPath(item));
            var builderBlock = (AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(item), typeof(BuilderBlock))) as BuilderBlock;
            if (builderBlock != null)
            {
                Debug.Log(builderBlock.gameObject.name);
                string path = AssetDatabase.GetAssetPath(builderBlock);
                AssetImporter assetImporter = AssetImporter.GetAtPath(path);
                assetImporter.assetBundleName = assetBundleName.ToLower();
            }
        }
    }
}
