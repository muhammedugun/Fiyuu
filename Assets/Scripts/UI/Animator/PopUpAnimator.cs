using DG.Tweening;
using UnityEngine;

public class PopUpAnimator : MonoBehaviour
{
    public float duration = 0.2f;

    void OnEnable()
    {
        transform.localScale = Vector3.zero;

        if (transform != null)
        {
            transform?.DOScale(Vector3.one, duration)
                 .SetEase(Ease.InOutQuad).SetUpdate(true);

        }
    }

}
