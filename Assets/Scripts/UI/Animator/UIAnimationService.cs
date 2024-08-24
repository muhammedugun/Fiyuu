using DG.Tweening;
using UnityEngine;

/// <summary>
/// UI Animasyonlar�yla ilgili s�n�flara hizmet eden s�n�ft�r
/// </summary>
public class UIAnimationService : MonoBehaviour
{
    public delegate void CallbackFunction();

    public static void ClosePopUp(RectTransform uiElement, float duration, bool setUpdate, CallbackFunction OnComplete = null, Ease ease = Ease.Linear)
    {
        uiElement.localScale = Vector3.one;
        PopUpAnimation(uiElement, duration, Vector3.zero, setUpdate, OnComplete, ease);
    }

    public static void OpenPopUp(RectTransform uiElement, float duration, bool setUpdate, CallbackFunction OnComplete = null, Ease ease = Ease.Linear)
    {
        uiElement.localScale = Vector3.zero;
        PopUpAnimation(uiElement, duration, Vector3.one, setUpdate, OnComplete, ease);
    }

    private static void PopUpAnimation(RectTransform uiElement, float duration, Vector3 endValue, bool setUpdate, CallbackFunction OnComplete = null, Ease ease=Ease.Linear)
    {
        uiElement?.DOScale(endValue, duration)
                   .SetEase(ease).OnComplete(() =>
                   {
                       OnComplete?.Invoke();

                   }).SetUpdate(setUpdate);
    }
}