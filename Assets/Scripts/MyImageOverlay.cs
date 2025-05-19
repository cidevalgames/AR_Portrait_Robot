using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using static UnityEngine.GraphicsBuffer;
using Vuforia;
using Unity.VisualScripting;

public class MyImageOverlay : MonoBehaviour
{
    [SerializeField] private OverlayType overlayType;

    private SpriteRenderer _spriteRenderer;
    private VideoPlayer _videoPlayer;
    private Animator _animator;
    private AudioSource _audioSource;
    private MeshRenderer[] _meshRenderers;

    private void Awake()
    {
        switch (overlayType)
        {
            case OverlayType.SpriteRenderer:
                _spriteRenderer = GetComponent<SpriteRenderer>();
                break;

            case OverlayType.VideoPlayer:
                _videoPlayer = GetComponentInChildren<VideoPlayer>();
                break;

            case OverlayType.Animator:
                _animator = GetComponentInChildren<Animator>();
                break;

            case OverlayType.MeshRenderers:
                _meshRenderers = GetComponentsInChildren<MeshRenderer>();
                break;

            case OverlayType.Undefined:
                throw new Exception("L'overlayType est défini sur Undefined.");
        }

        if (_audioSource)
        {
            _audioSource.playOnAwake = false;
            _audioSource.enabled = false;
        }
    }

    /// <summary>
    /// Active ou désactive l'overlay.
    /// </summary>
    /// <param name="enable">Etat d'affichage de l'overlay</param>
    public void SetOverlay(bool enable)
    {
        //Debug.Log($"Enable: {enable}");

        switch (overlayType)
        {
            case OverlayType.SpriteRenderer:
                _spriteRenderer.enabled = enable;
                break;

            case OverlayType.VideoPlayer:
                _videoPlayer.GetComponent<MeshRenderer>().enabled = enable;
                _videoPlayer.enabled = enable;

                if (enable)
                    _videoPlayer.Play();
                break;

            case OverlayType.Animator:
                _animator.GetComponent<SpriteRenderer>().enabled = enable;
                _animator.enabled = enable;
                break;

            case OverlayType.MeshRenderers:
                if (_meshRenderers.Length > 0)
                    foreach (MeshRenderer m in _meshRenderers)
                        m.enabled = enable;
                break;

            case OverlayType.Undefined:
                throw new Exception("L'overlayType est défini sur Undefined.");
        }

        if (_audioSource)
        {
            _audioSource.enabled = enable;

            if (enable)
                _audioSource.Play();
        }
    }
}
