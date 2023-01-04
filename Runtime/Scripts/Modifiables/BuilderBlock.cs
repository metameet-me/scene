using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BuilderBlock : Clickable
{
    public CustomObject CustomObject;
    private Texture Thumbnail;
    public string ThumbnailName; // This determines the relative path from the base image folder on the blob, so "/" can be used to target subfolders.
    public string DisplayName;
    public string Category;
    public Action OnClickAction;
    public Action InitializeAction;
    public Action OnDestroyAction;
    public Action MouseEnterAction;
    public Action MouseExitAction;

    public bool Scalable = true;
    public Vector2 MinMaxScaleMultiplier;

    [HideInInspector]
    public string AssetName;
    [HideInInspector]
    public string AssetBundle;

    [HideInInspector]
    public string GroupName;

    [SerializeField]
    public List<MeshRenderer> _meshesToOutline = new List<MeshRenderer>();
    public List<MeshRenderer> MeshesToOutline { get { return _meshesToOutline; } }




    public void Initialize()
    {
        if (InitializeAction != null)
            InitializeAction.Invoke();
    }

    public void Interactable(bool enabled)
    {
        GetComponent<Collider>().enabled = enabled;
    }

    private void OnDestroy()
    {
        if (OnDestroyAction != null)
            OnDestroyAction.Invoke();
    }

    public override void OnClick()
    {
        if (OnClickAction != null)
            OnClickAction.Invoke();
    }

    public override void MouseEnter()
    {
        if (MouseEnterAction != null)
            MouseEnterAction.Invoke();

    }

    public override void MouseOver()
    {
    }

    public override void MouseExit()
    {
        if (MouseExitAction != null)
            MouseExitAction.Invoke();

    }
}
