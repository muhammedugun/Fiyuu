using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Buton animasyonlarýndan sorumlu
/// </summary>
public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _duration = 0.2f;

    private RectTransform _rectTransform;
    private float _defaultWidth;
    private float _defaultHeight;
   
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _defaultWidth = _rectTransform.sizeDelta.x;
        _defaultHeight = _rectTransform.sizeDelta.y;
    }

    private void OnEnable()
    {
        if (_rectTransform != null)
        {
            _rectTransform?.DOSizeDelta(new Vector2(_defaultWidth, _defaultHeight), _duration)
                    .SetEase(Ease.InOutQuad).SetUpdate(true);
        } 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonAnimation();
    }

    public void ButtonAnimation()
    {
        float targetWidth = _defaultWidth * 1.15f;
        float targetHeight = _defaultHeight * 1.15f;

        if (_rectTransform != null)
        {
            _rectTransform?.DOSizeDelta(new Vector2(targetWidth, targetHeight), _duration)
                 .SetEase(Ease.InOutQuad).SetUpdate(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_rectTransform != null)
        {
            _rectTransform?.DOSizeDelta(new Vector2(_defaultWidth, _defaultHeight), _duration)
                 .SetEase(Ease.InOutQuad).SetUpdate(true);
        }
    }
}
