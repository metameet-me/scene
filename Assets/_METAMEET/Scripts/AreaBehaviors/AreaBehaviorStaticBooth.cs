﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AreaBehaviorStaticBooth : AreaBehaviour
{
    [SerializeField]
    private BoxCollider _blockingObject;
    public string AreaName;
 }
