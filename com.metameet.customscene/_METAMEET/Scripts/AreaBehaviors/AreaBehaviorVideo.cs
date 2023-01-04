using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class AreaBehaviorVideo : AreaBehaviour
{
    public bool withSound;
    public bool singlePlay;
    public bool stopOnStart; //FYI I hate this naming just did it so I don't have to update on every booth
    public bool zoom;
    public bool stopAgentMovement = false;
}
