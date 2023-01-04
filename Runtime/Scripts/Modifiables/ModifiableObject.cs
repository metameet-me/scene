using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]

public class ModifiableObject : Clickable
{
    [Header("Will it be hidden while not in edit mode")]
    public bool AlwaysShow = false;

    [Header("Editing Position and Scale")]
    public bool Editable = false;

    [Header("Reference needed")]
    public Transform ParentObject;

    [Header("(Automatically Set) Item belongs to a Room")]
    public bool RoomItem;
    [Header("(Automatically Set) Item belongs to a Booth")]
    public GameObject Booth;
    [Header("Possible objects")]
    public bool CanAddImage = true;
    public bool CanAddVideo = true;
    public bool CanDownloadLink = true;
    public bool EpikAPI = false;


    public Material CurrentMaterial;

    public Vector3 MeshBoundsSize;

    public bool Loaded = false;

    [Header("Loading parameters")]
    public float DistanceToLoad = 100f;

    public override void OnClick()
    {
        throw new System.NotImplementedException();
    }
}
