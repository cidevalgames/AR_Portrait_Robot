using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ZoomableImage : MonoBehaviour
{
    [SerializeField] private Vector2 zoomedSize = Vector2.zero;
    [SerializeField, Range(.1f, 1f)] private float zoomDuration = .5f;
    [SerializeField] private CanvasGroup fadedBackground;

    private Button m_button;
    private RectTransform m_rectTransform;
    private GraphicRaycaster m_graphicRaycaster;
    private Canvas m_canvas;
    private Canvas _fadedBackgroundCanvas;

    private bool _isAnimating = false;
    private bool _isZoomed = false;
    private Vector2 _baseSize, _basePos;
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

    private IEnumerator Start()
    {
        yield return null;

        _baseSize = m_rectTransform.sizeDelta;
        _basePos = m_rectTransform.anchoredPosition;
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

        m_graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
        m_canvas = GetComponent<Canvas>();
        m_canvas.overrideSorting = true;
        m_canvas.sortingOrder = 3;
        
        _isAnimating = true;

        fadedBackground.DOFade(.7f, zoomDuration).SetEase(Ease.OutExpo);

        m_rectTransform.DOMove(_targetPos, zoomDuration).SetEase(Ease.OutExpo);

        m_rectTransform.DOSizeDelta(zoomedSize, zoomDuration).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            _isAnimating = false;
            _isZoomed = true;
        });
    }

    private void DezoomImage()
    {
        if (_isAnimating)
            return;        

        _isAnimating = true;

        fadedBackground.alpha = .7f;

        fadedBackground.DOFade(0f, zoomDuration).SetEase(Ease.OutExpo);

        m_rectTransform.DOAnchorPos(_basePos, zoomDuration).SetEase(Ease.OutExpo);

        m_rectTransform.DOSizeDelta(_baseSize, zoomDuration).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            Destroy(m_graphicRaycaster);
            Destroy(m_canvas);
            GetComponentInParent<GridLayoutGroup>().enabled = true;
            _fadedBackgroundCanvas.sortingOrder = 1;

            _isAnimating = false;
            _isZoomed = false;

            foreach (var button in _everyButton)
            {
                if (button != m_button)
                {
                    button.interactable = true;
                }
            }
        });
    }

    private void OnClick_Button()
    {
        if (!_isZoomed)
            ZoomImage();
        else
            DezoomImage();
    }
}
