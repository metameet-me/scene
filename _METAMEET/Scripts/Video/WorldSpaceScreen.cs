using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceScreen : Clickable
{
    public string screenid;
    public AreaBehaviour areaBehavior;
    public bool AudioShareScreen;

    private GameObject _paintedObject;

    public Transform CameraPositionThis, CameraPositionAll;

    public override void OnClick()
    {
        throw new System.NotImplementedException();
    }
}
