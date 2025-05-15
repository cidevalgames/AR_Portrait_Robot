using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ARSceneButton : MonoBehaviour
{
    [SerializeField] private Vector2 zoomedSize = Vector2.zero;
    [SerializeField, Range(.1f, 1f)] private float zoomDuration = .5f;
    [SerializeField] private CanvasGroup fadedBackground;

    private Button m_button;
    private RectTransform m_rectTransform;
    private Canvas m_canvas;
    private Canvas _fadedBackgroundCanvas;

    private bool _isAnimating = false;
    private Vector2 _targetPos;

    private Button[] _everyButton;

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_button = GetComponent<Button>();
        _fadedBackgroundCanvas = fadedBackground.GetComponent<Canvas>();

        _everyButton = FindObjectsOfType<Button>();

        _targetPos = new Vector2(Screen.width / 2, Screen.height / 2);

        m_button.onClick.AddListener(OnClick_Button);
    }

    private void ZoomImage()
    {
        Debug.Log("Zoom");

        if (_isAnimating)
            return;

        foreach (var button in _everyButton)
        {
            if (button != m_button)
            {
                button.interactable = false;
            }
        }

        GetComponentInParent<GridLayoutGroup>().enabled = false;

        fadedBackground.alpha = 0f;
        _fadedBackgroundCanvas.sortingOrder = 2;

        m_canvas = gameObject.AddComponent<Canvas>();
        m_canvas.overrideSorting = true;
        m_canvas.sortingOrder = 3;

        _isAnimating = true;

        fadedBackground.DOFade(.7f, zoomDuration).SetEase(Ease.OutExpo);

        m_rectTransform.DOMove(_targetPos, zoomDuration).SetEase(Ease.OutExpo);

        m_rectTransform.DOSizeDelta(zoomedSize, zoomDuration).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            SceneManager.LoadScene("Portrait_Robot");
        });
    }

    private void OnClick_Button()
    {
        ZoomImage();
    }
}
