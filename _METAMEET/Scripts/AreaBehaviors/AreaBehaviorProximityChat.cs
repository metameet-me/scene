
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
public class AreaBehaviorProximityChat : AreaBehaviour
{
#if UNITY_EDITOR    
    [field: SerializeField, ReadOnlyField]
#endif
    private BoxCollider _blockingObject;
    public string AreaName;

    public List<MetameetVideoApply> Screens;
#if UNITY_EDITOR    
    [field: SerializeField, ReadOnlyField]
#endif
    public GameObject AreaPrefixNameObject;
}
