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

    private SpriteRenderer _spriteRenderer;
    private BoxCollider _collider;
    private VideoPlayer _videoPlayer;
    private Animator _animator;
    private AudioSource _audioSource;

    private void Awake()
    {
        _observer = GetComponent<DefaultObserverEventHandler>();
        _target = GetComponent<ImageTargetBehaviour>();

        _spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        _collider = GetComponent<BoxCollider>();
        _videoPlayer = GetComponentInChildren<VideoPlayer>();
        _animator = GetComponentInChildren<Animator>();
        _audioSource = GetComponentInChildren<AudioSource>();

        if (_audioSource)
        {
            _audioSource.playOnAwake = false;
            _audioSource.enabled = false;
        }

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
        if (_spriteRenderer)
            _spriteRenderer.enabled = true;

        if (_videoPlayer)
        {
            _videoPlayer.GetComponent<MeshRenderer>().enabled = true;
            _videoPlayer.enabled = true;
            _videoPlayer.Play();
        }

        if (_animator)
        {
            _animator.GetComponent<SpriteRenderer>().enabled = true;
            _animator.enabled = true;
        }

        if (_audioSource)
        {
            _audioSource.enabled = true;
            _audioSource.Play();
        }
    }

    public void DisableImage()
    {
        if (_spriteRenderer)
            _spriteRenderer.enabled = false;

        if (_videoPlayer)
        {
            _videoPlayer.enabled = false;
            _videoPlayer.GetComponent<MeshRenderer>().enabled = false;
        }

        if (_animator)
        {
            _animator.GetComponent<SpriteRenderer>().enabled = false;
            _animator.enabled = false;
        }

        if (_audioSource)
        {
            _audioSource.enabled = false;
        }
    }

    private void Event_OnTargetFound()
    {
        _collider.enabled = true;

        if (_spriteRenderer)
            _spriteRenderer.enabled = false;
        if (_videoPlayer)
            _videoPlayer.enabled = false;
    }

    public void Event_OnTargetLost()
    {
        _collider.enabled = false; 

        if (_spriteRenderer)
            _spriteRenderer.enabled = false;
        if (_videoPlayer)
            _videoPlayer.enabled = false;
    }
}
