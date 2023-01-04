using System;
using UnityEngine;

[Serializable]
public class Seat : MonoBehaviour
{
    public int Index;
    public SeatInteractable ParentInteractable;
    public Collider Collider;
    public bool Available;
    public bool Teleported;
}