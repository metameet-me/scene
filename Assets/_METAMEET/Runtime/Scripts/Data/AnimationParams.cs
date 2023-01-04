using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum AnimationParams
{
    IdleWatch = -3,
    IdleScratch = -2,
    IdleLookOver = -1,
    Idle = 0,
    Sit = 1,
    Lay = 2,

    WalkForward = 3,
    Sprint = 33,
    WalkBackward = 4,
    TurnRight = 5,
    TurnLeft = 6,

    RaiseHand = 7,
    Wave = 8,
    RaiseHandSitting = 9,
    Clap = 10,
    ClapSitting = 11,

    Shrug = 12,
    ThumbsDown = 13,
    ThumbsUp = 14,

    Dance0 = 18,
    Dance1 = 19,
    Dance2 = 20,
    Dance3 = 21,
    Dance4 = 22
}
