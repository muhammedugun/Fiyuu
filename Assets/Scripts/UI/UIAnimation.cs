using DG.Tweening;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    public delegate void CallbackFunction();

    public static void OpenPopUp(RectTransform uiElement, float duration, CallbackFunction OnComplete)
    {
        uiElement?.DOScale(Vector3.zero, duration)
                   .SetEase(Ease.InOutQuad).OnComplete(() =>
                   {
                       OnComplete();
                   });
    }
}
