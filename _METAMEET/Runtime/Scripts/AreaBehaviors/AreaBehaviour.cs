using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public enum AreaType 
{
    Predefined, //AI reception, Avatar creation, ect...
    Socializing, //Open spaces for hangout
    Promotional, //Can be forced or not to speak with promoters
    Presentation, //Focused on one person talking
    Transitioning
}

public enum AreaChatType 
{
    GroupChat, //if not in any chat already, invite to group chat of that area
    ForceGroupChat, //enforce player to leave current chat if any, and join group chat
    ForceLeaveChat
}

public enum AreaViewType 
{
    FirstPerson,
    ForceFirstPerson,
    ThirdPerson,
    ForceThirdPerson
}

[Serializable]
public class AreaBehaviour : MonoBehaviour, IArea
{
    public AreaType AreaType;

    [Header("Settings"), Space(10)]
#if UNITY_EDITOR    
    [field: SerializeField, ReadOnlyField]
    #endif
    public bool AllowedMovement = true;
#if UNITY_EDITOR    
    [field: SerializeField, ReadOnlyField]
#endif
    public bool FocusCamera = false;
#if UNITY_EDITOR    
    [field: SerializeField, ReadOnlyField]
#endif
    public bool FocusFreeze = true;

    public Transform FocusTarget = null;
#if UNITY_EDITOR    
    [field: SerializeField, ReadOnlyField]
#endif
    public Transform ReferenceCameraPosition = null;

    [Header("Actions"), Space(10)]
#if UNITY_EDITOR    
    [field: SerializeField, ReadOnlyField]
#endif
    public UnityEvent ActionWhenPlayerEnters;
#if UNITY_EDITOR    
    [field: SerializeField, ReadOnlyField]
#endif
    public UnityEvent ActionWhenPlayerExits;
#if UNITY_EDITOR    
    [field: SerializeField, ReadOnlyField]
#endif
    public UnityEvent PrepareContent;
#if UNITY_EDITOR    
    [field: SerializeField, ReadOnlyField]
#endif
    public UnityEvent ReleaseContent;
}

#if UNITY_EDITOR
[UsedImplicitly, CustomPropertyDrawer(typeof(ReadOnlyFieldAttribute))]
public class ReadOnlyFieldAttributeDrawer : PropertyDrawer
{
}

[System.AttributeUsage(System.AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class ReadOnlyFieldAttribute : PropertyAttribute { }
#endif