using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[ExecuteInEditMode]
public class SeatInteractable : Clickable
{
    [SerializeField, Tooltip("Initialize seats if they dont belong to any Interactable Collection")]
    private bool _autoInitialize = false;


    public AnimationParams StationaryPoseAnimation = AnimationParams.Sit;

    [SerializeField]
    private List<MeshRenderer> _meshesToOutline = new List<MeshRenderer>();

    [Space(10), Header("For editing seats!"), SerializeField]
    private bool _editMode;

    [SerializeField, Tooltip("In order to interact with a Seat, it must have its own Collider or copy Colliders from the meshes it will paint")]
    private bool _copyCollidersFromMeshes;

    public Vector3 Offset = Vector3.zero;
    public Vector3 Rotation = Vector3.zero;

    [SerializeField]
    private float _leaveDelay = 0f;

    [SerializeField]
    private float _seatSize = 0.5f;

    [Space(10), SerializeField]
    private bool _showAvatarExample;

#if UNITY_EDITOR
    private GameObject _exampleAvatar;
    private Transform _seatPositionsParent;
    private Seat _seat;
#endif

    public override void OnClick()
    {
        throw new NotImplementedException();
    }

    void LateUpdate()
    {
#if UNITY_EDITOR
        if (_editMode)
        {
            _seatPositionsParent = transform.Find("Seat Positions");

            if (_seatPositionsParent == null)
            {
                _seatPositionsParent = new GameObject("Seat Positions").transform;
                _seatPositionsParent.SetParent(transform);
            }

            Seat seat = _seatPositionsParent.Find("Seat_Object")?.GetComponent<Seat>();
            if (seat != null)
            {
                _seat = seat;
            }
            else
            {
                Transform seatObject = new GameObject("Seat_Object").transform;
                seatObject.transform.SetParent(_seatPositionsParent);
                seatObject.gameObject.layer = LayerMask.NameToLayer("ZoneTriggers");

                BoxCollider collider = seatObject.gameObject.AddComponent<BoxCollider>();
                collider.size = Vector3.one * _seatSize;
                collider.center = new Vector3(0f, _seatSize * 0.5f, 0f);
                collider.isTrigger = true;

                _seat = seatObject.gameObject.AddComponent<Seat>();
                _seat.ParentInteractable = this;
                _seat.Collider = collider;
                _seat.Available = true;
            }
            Vector3 seatPos = _seat.transform.position;
            _seatPositionsParent.transform.localPosition = Vector3.zero;
            _seat.transform.position = seatPos;

            if (_seat != null)
            {

                if (_seat == null)
                    return;

                if ((_seat.Collider as BoxCollider).size.magnitude != _seatSize)
                {
                    (_seat.Collider as BoxCollider).size = Vector3.one * _seatSize;
                    (_seat.Collider as BoxCollider).center = new Vector3(0f, _seatSize * 0.5f, 0f);
                }

            }


            if (_copyCollidersFromMeshes)
            {
                foreach (var currentCollider in gameObject.GetComponents<Collider>())
                {
                    if (currentCollider != null)
                    {
                        DestroyImmediate(currentCollider);
                    }
                }

                foreach (var mesh in _meshesToOutline)
                {
                    foreach (var collider in mesh.GetComponents<Collider>())
                    {
                        CopyComponent(collider, gameObject);
                    }
                }

                _copyCollidersFromMeshes = false;

            }

            if (_showAvatarExample)
            {
                if (_exampleAvatar != null)
                {
                    _exampleAvatar.transform.position = _seat.transform.position + Offset;
                    _exampleAvatar.transform.rotation = _seat.transform.rotation * Quaternion.Euler(Rotation);
                    return;
                }

                GameObject avatar = Instantiate(Resources.Load<GameObject>("SitExampleMesh"), _seat.transform);

                avatar.transform.position = _seat.transform.position;
                avatar.transform.rotation = _seat.transform.rotation;

                _exampleAvatar = avatar;
            }
            else
            {
                if (_exampleAvatar != null)
                {
                    DestroyImmediate(_exampleAvatar);
                }
            }
        }
#endif
    }

    private static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        Type type = original.GetType();
        Component copy = destination.AddComponent(type);

        FieldInfo[] fields = type.GetFields(BindingFlags.Instance |
                       BindingFlags.Static |
                       BindingFlags.NonPublic |
                       BindingFlags.Public);

        if (fields != null)
        {
            foreach (FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
        }


        PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance |
                       BindingFlags.Static |
                       BindingFlags.NonPublic |
                       BindingFlags.Public);

        if (properties != null)
        {
            foreach (PropertyInfo property in properties)
            {
                if (property.Name == "isTrigger" || property.Name == "name")
                {
                    continue;
                }
                if (property.CanWrite)
                {
                    property.SetValue(copy, property.GetValue(original));
                }
            }
        }


        return copy as T;
    }
}

