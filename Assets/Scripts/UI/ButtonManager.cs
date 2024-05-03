using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform rectTransform;
    float defaultWidth;
    float defaultHeight;
    [SerializeField] float duration = 0.2f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        defaultWidth = rectTransform.sizeDelta.x;
        defaultHeight = rectTransform.sizeDelta.y;
    }
    // UI butonunun üzerine gelindiðinde çaðrýlan iþlev
    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonAnimation();
    }

    public void ButtonAnimation()
    {
        float targetWidth = defaultWidth * 1.15f;
        float targetHeight = defaultHeight * 1.15f;


        rectTransform.DOSizeDelta(new Vector2(targetWidth, targetHeight), duration)
                 .SetEase(Ease.InOutQuad).SetUpdate(true);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOSizeDelta(new Vector2(defaultWidth, defaultHeight), duration)
                 .SetEase(Ease.InOutQuad).SetUpdate(true);
    }
}
