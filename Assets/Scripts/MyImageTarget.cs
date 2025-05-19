using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Video;

#if UNITY_EDITOR
using UnityEditor;
#endif
using Vuforia;

[RequireComponent(typeof(BoxCollider))]
public class MyImageTarget : MonoBehaviour
{
    private DefaultObserverEventHandler _observer;
    private ImageTargetBehaviour _target;

    public MyImageOverlay[] imageOverlays { get; private set; } = new MyImageOverlay[0];

    private BoxCollider _collider;

    private int _currentOverlayIndex = 0;

    private void Awake()
    {
        _observer = GetComponent<DefaultObserverEventHandler>();
        _target = GetComponent<ImageTargetBehaviour>();

        _collider = GetComponent<BoxCollider>();

        imageOverlays = GetComponentsInChildren<MyImageOverlay>(true);

        if (_collider.size == Vector3.one)
            AdaptColliderSize();

        _observer.OnTargetFound.AddListener(Event_OnTargetFound);
        _observer.OnTargetLost.AddListener(Event_OnTargetLost);
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

#if UNITY_EDITOR
        _collider.enabled = true;

        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>(true);

        foreach (MeshRenderer m in meshes)
        {
            if (m.name.Contains("Target Representation"))
            {
                m.enabled = true;
            }
            else if (m.name.Contains("Track_Indicator"))
            {
                m.gameObject.SetActive(true);
                m.enabled = true;
            }
        }
#endif
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

        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>(true);

        foreach (MeshRenderer m in meshes)
        {
            if (m.name.Contains("Track_Indicator"))
            {
                m.transform.localScale = new Vector3(imageSize.x, .05f, imageSize.y);
            }
        }
#endif
    }

    public void ShowImage()
    {
        if (imageOverlays.Length == 0)
            return;

        foreach (var overlay in imageOverlays)
            overlay.SetOverlay(false);

        imageOverlays[_currentOverlayIndex % imageOverlays.Length].SetOverlay(true);

        _currentOverlayIndex++;
    }

    public void DisableImage()
    {
        //Debug.Log($"Image overlays: {_imageOverlays.Length}");

        foreach (var overlay in imageOverlays)
            overlay.SetOverlay(false);

        _currentOverlayIndex = 0;
    }

    private void Event_OnTargetFound()
    {
        _collider.enabled = true;

        DisableImage();
    }

    public void Event_OnTargetLost()
    {
        _collider.enabled = false; 

        DisableImage();
    }
}
