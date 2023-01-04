//#if !UNITY_WEBGL
//using agora_gaming_rtc;
//#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MetameetVideoApply : MonoBehaviour
{
    public RawImage image;
    private MeshRenderer mesh;
    public string _uid = null;
    public Texture DefaultTexture;
    public Texture MuteTexture;
}
