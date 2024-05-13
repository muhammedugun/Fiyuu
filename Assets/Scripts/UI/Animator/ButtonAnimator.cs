using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform rectTransform;
    float defaultWidth;
    float defaultHeight;
    [SerializeField] float duration = 0.2f;

    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        defaultWidth = rectTransform.sizeDelta.x;
        defaultHeight = rectTransform.sizeDelta.y;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonAnimation();
    }

    public void ButtonAnimation()
    {
        float targetWidth = defaultWidth * 1.15f;
        float targetHeight = defaultHeight * 1.15f;

        if (rectTransform != null)
        {
            rectTransform?.DOSizeDelta(new Vector2(targetWidth, targetHeight), duration)
                 .SetEase(Ease.InOutQuad).SetUpdate(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (rectTransform != null)
        {
            rectTransform?.DOSizeDelta(new Vector2(defaultWidth, defaultHeight), duration)
                 .SetEase(Ease.InOutQuad).SetUpdate(true);
        }
    }
}
