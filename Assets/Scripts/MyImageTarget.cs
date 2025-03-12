using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Vuforia;

[RequireComponent(typeof(BoxCollider))]
public class MyImageTarget : MonoBehaviour
{
    private DefaultObserverEventHandler _observer;
    private ImageTargetBehaviour _target;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider _collider;

    private void Awake()
    {
        _observer = GetComponent<DefaultObserverEventHandler>();
        _target = GetComponent<ImageTargetBehaviour>();

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _collider = GetComponent<BoxCollider>();

        if (_collider.size == Vector3.one)
            AdaptColliderSize();

        _observer.OnTargetFound.AddListener(Event_OnTargetFound);
        _observer.OnTargetLost.AddListener(Event_OnTargetLost);
    }

    [ContextMenu("Adapt collider")]
    public void AdaptColliderSize()
    {
        _target = GetComponent<ImageTargetBehaviour>();
        _collider = GetComponent<BoxCollider>();

        Vector2 imageSize = _target.GetSize();

        _collider.size = new Vector3(imageSize.x, .05f, imageSize.y);

        Debug.Log($"Readapted collider on {name} object.");

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    private void Event_OnTargetFound()
    {
        _collider.enabled = true;
        _spriteRenderer.enabled = false;
    }

    private void Event_OnTargetLost()
    {
        _collider.enabled = false; 
        _spriteRenderer.enabled = false;
    }
}
