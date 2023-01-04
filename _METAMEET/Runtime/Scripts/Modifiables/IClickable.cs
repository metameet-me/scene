using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable
{
    bool Outline { get; set; }

    void MouseEnter();
    void MouseOver();
    void MouseExit();
    void OnClick();
}
