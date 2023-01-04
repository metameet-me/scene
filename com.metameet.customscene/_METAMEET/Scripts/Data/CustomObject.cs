using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomObject : Entity
{

    public CustomObject()
    {
    }
    public string Tag { get; set; }
    public string AssetBundle { get; set; }
    public string AssetName { get; set; }
    public float? PositionX { get; set; }
    public float? PositionY { get; set; }
    public float? PositionZ { get; set; }
    public float? ScaleX { get; set; }
    public float? ScaleY { get; set; }
    public float? ScaleZ { get; set; }
    public float? ColorR { get; set; }
    public float? ColorG { get; set; }
    public float? ColorB { get; set; }
    public float? TextureOffsetX { get; set; }
    public float? TextureOffsetY { get; set; }
    public float? TextureScaleX { get; set; }
    public float? TextureScaleY { get; set; }
    public float? RotationX { get; set; }
    public float? RotationY { get; set; }
    public float? RotationZ { get; set; }
    public float? RotationW { get; set; }

}
