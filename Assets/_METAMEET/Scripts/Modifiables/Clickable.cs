using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public abstract class Clickable : MonoBehaviour, IClickable
{
    public bool Outline { get; set; }

    [Header("Left = Available ||| Right = Unavailable")]
    public Gradient OutlineColor;

    public abstract void OnClick();
    public virtual void OnClickByOther()
    {

    }

    public virtual void MouseEnter()
    {

    }

    public virtual void MouseOver()
    {
       
    }

    public virtual void MouseExit()
    {

    }

    //private HighlightingSystem.Highlighter highlighter;
}
