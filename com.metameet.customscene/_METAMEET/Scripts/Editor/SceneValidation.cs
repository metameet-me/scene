using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneValidation : MonoBehaviour
{
    public static bool ValidateScene()
    {
        Scene scene = EditorSceneManager.GetActiveScene();
        bool success = true;
         if(success) success =  ValidateModifiables(scene);
        if (success) success = ValidatePresentations(scene);
        if (success) success = ValidateChatAreas(scene);
        if (success)
            Debug.LogFormat("<color=green>SUCCESS!</color> Your Scene in valid and ready to upload <color=yellow>{0}</color>  Good Job!", Path.GetFileNameWithoutExtension(scene.path));
        else
            Debug.LogFormat("<color=red>FAILED!</color> Please fix all the issues for Scene: <color=yellow>{0}</color>!", Path.GetFileNameWithoutExtension(scene.path));
        return success;
    }
    public static bool ValidateSceneScripts(Scene scene)
    {
        bool success = CheckPresentations(scene);
        return success;
    }
    public static bool ValidatePresentations(Scene scene)
    {
        bool success = CheckPresentations(scene);
        return success;
    }
    public static bool ValidateModifiables(Scene scene)
    {
        bool success = ModifiableObjectsOnScenePrint(scene);
        return success;
    }

    public static bool ValidateChatAreas(Scene scene)
    {
        bool success = CheckChatAreas(scene);
        return success;
    }

    private static bool CheckSceneScripts(Scene scene)
    {
        bool success = true;
        var sceneLoaders = FindObjectsOfType<MetameetSceneLoader>().ToList();

        if(sceneLoaders.Count > 1)
        {
            Debug.Log("Scripts: <color=red>" + sceneLoaders.First().gameObject.name + "</color> must have only one scene loader!!!");
            success = false;
        }
        var sceneLoader = sceneLoaders.First();

        if(sceneLoader.InitialSpawnPosition == null)
        {
            Debug.Log("Spawn Positions: <color=red>" + sceneLoader.gameObject.name + "</color> must set a transform for spawn positions!!!");
            success = false;
        }

        if (sceneLoader.InitialSpawnPosition == null)
        {
            Debug.Log("Spawn Positions: <color=red>" + sceneLoader.gameObject.name + "</color> must set a transform for spawn positions!!!");
            success = false;
        }

        return success;
    }
    private static bool CheckPresentations(Scene scene)
    {
        bool success = true;
        var presentations = FindObjectsOfType<AreaBehaviorPresentation>().ToList();
        foreach (var item in presentations)
        {
            if(item.gameObject.layer != 10)
            {
                Debug.Log("Presentation: <color=red>" + item.gameObject.name + "</color> must be on the ZoneTriggers Layer!!!");
                success = false;
            }
            if(string.IsNullOrEmpty(item.name))
            {
                Debug.Log("Presentation: <color=red>" + item.gameObject.name + "</color> must have a stage name!!!");
                success = false;
            }
            if (item.Screens.Count(s=>s != null) <1)
            {
                Debug.Log("Presentation: <color=red>" + item.gameObject.name + "</color> must have at least one screen!!!");
                success = false;
            }
            if (item.Screens.Count(s => s == null) > 0)
            {
                Debug.Log("Presentation: <color=red>" + item.gameObject.name + "</color> must not have a null value for a screen!!!");
                success = false;
            }
            foreach (var screen in item.Screens)
            {
                WorldSpaceScreen wps = screen.GetComponent<WorldSpaceScreen>();
                if(wps.CameraPositionThis == null)
                {
                    Debug.Log("Presentation Screen: <color=red>" + screen.gameObject.name + "</color> is missing a camera position for this screen!!!");
                    success = false;
                }
                if (wps.CameraPositionAll == null)
                {
                    Debug.Log("Presentation Screen: <color=red>" + screen.gameObject.name + "</color> is missing a camera position for all screens screen!!!");
                    success = false;
                }

                MeshRenderer mesh = screen.GetComponent<MeshRenderer>();
                if(mesh.sharedMaterial.name != "VideoUnlitMaterial")
                {
                    Debug.Log("Presentation Screen: <color=red>" + screen.gameObject.name + "</color> please use the VideoUnlitMaterial for a presentation screen!!!");
                    success = false;
                }
            }

       

        }

        var groupbyname = presentations.GroupBy(d => d.name);
        var groupdup = groupbyname.Where(d => d.Count() > 1).ToList();
        if (groupdup.Count() > 0)
        {
            foreach (var dup in groupdup)
            {
                Debug.Log("Presentation: <color=red>" + dup.First().gameObject.name + "</color> Duplicate presentation name. Presentation names must be unique!!!");
                success = false;
            }

        }
        return success;
    }



    private static bool CheckChatAreas(Scene scene)
    {
        bool success = true;
        var chatarea = FindObjectsOfType<AreaBehaviorProximityChat>().ToList();
        foreach (var item in chatarea)
        {
            if (item.gameObject.layer != 10)
            {
                Debug.Log("Chat Area: <color=red>" + item.gameObject.name + "</color> must be on the ZoneTriggers Layer!!!");
                success = false;
            }
            var collider = item.gameObject.GetComponent<BoxCollider>();
            if (collider == null)
            {
                Debug.Log("Chat Area: <color=red>" + item.gameObject.name + "</color> must have a box colider!!!");
                return success;
            }
            if (!collider.isTrigger )
            {
                Debug.Log("Chat Area: <color=red>" + item.gameObject.name + "</color> box collider must have IsTrigger set to true !!!");
                success = false;
            }

            var renderer = item.gameObject.GetComponent<MeshRenderer>();
            if(renderer != null && renderer.enabled)
            {
                Debug.Log("Chat Area: <color=red>" + item.gameObject.name + "</color> mesh renderer should not be enabled !!!");
                success = false;
            }

            
            if (item.Screens.Count(s => s == null) > 0)
            {
                Debug.Log("Chat Area: <color=red>" + item.gameObject.name + "</color> must not have a null value for a screen!!!");
                success = false;
            }
            foreach (var screen in item.Screens)
            {
                WorldSpaceScreen wps = screen.GetComponent<WorldSpaceScreen>();
                if (wps.CameraPositionThis == null)
                {
                    Debug.Log("Chat Area Screen: <color=red>" + screen.gameObject.name + "</color> is missing a camera position for this screen!!!");
                    success = false;
                }

                MeshRenderer mesh = screen.GetComponent<MeshRenderer>();
                if (mesh.sharedMaterial.name != "VideoUnlitMaterial")
                {
                    Debug.Log("Chat Area Screen: <color=red>" + screen.gameObject.name + "</color> please use the VideoUnlitMaterial for a presentation screen!!!");
                    success = false;
                }
            }

        
        }

        var groupbyname = chatarea.GroupBy(d => d.AreaName);
        var groupdup = groupbyname.Where(d => d.Count() > 1).ToList();
        if (groupdup.Count() > 0)
        {
            foreach (var dup in groupdup)
            {
                Debug.Log("Chat Area: <color=red>" + dup.First().gameObject.name + "</color> Duplicate chat area name. Presentation names must be unique!!!");
                success = false;
            }

        }
        return success;
    }

    public static void ValidateAssetBundles()
    {

    }

    private static bool ModifiableObjectsOnScenePrint(Scene scene)
    {
        bool success = true;

        var modifiableObjects = GameObject.FindObjectsOfType<ModifiableObject>().ToList();
        modifiableObjects.Where(x => x.Booth == null).ToList();
        Debug.LogFormat("Scene: <color=yellow>{0}</color> --- Modifiable Objects Count: {1}", Path.GetFileNameWithoutExtension(scene.path), modifiableObjects.Count);

        // Check layers
        foreach (var item in modifiableObjects)
        {
            // If it's a world screen, the layer is clickable, so ignore world screens
            var worldScreen = item.GetComponent<WorldSpaceScreen>();
            if (worldScreen == null)
            {
                if (item.gameObject.layer != 11)
                {
                    Debug.Log("Gameobject: <color=red>" + item.gameObject.name + "</color> is not having proper layer. Please assign Modifiable layer!!!");
                    success = false;
                }
            }
        }

        List<ModifiableObject> withoutParent = modifiableObjects.Where(x => x.ParentObject == null).ToList();
        foreach (var mod in withoutParent)
        {
            Debug.LogFormat("Scene: <color=yellow>{0}</color> --- Parent object is missing in Modifiable Object named: <color=red>{1}</color>", Path.GetFileNameWithoutExtension(scene.path), mod.name);
            success = false;
        }

        var duplicates = modifiableObjects.Where(w => w.ParentObject != null).GroupBy(x => x.name + x.ParentObject.name)
                        .Where(g => g.Count() > 1)
                        .Select(g => g).ToList();


        foreach (var duplicate in duplicates)
        {
            var item = duplicate.FirstOrDefault();
            Debug.LogFormat("Scene: <color=yellow>{0}</color> --- Name or Parent name must be unique for Modifiable Object named: <color=red>{1}</color> Parent named : <color=red>{2}</color> Duplicate Count: <color=yellow>{3}</color>", Path.GetFileNameWithoutExtension(scene.path), item.name, item.ParentObject.name, duplicate.Count());
            success = false;
        }

        if (success)
            Debug.LogFormat("<color=green>SUCCESS!</color> All Modifiable Objects in <color=yellow>{0}</color> have a unique name! Good Job!", Path.GetFileNameWithoutExtension(scene.path));
        else
            Debug.LogFormat("<color=red>FAILED!</color> Please fix all the issues for Scene: <color=yellow>{0}</color>!", Path.GetFileNameWithoutExtension(scene.path));

        return success;
    }
}
