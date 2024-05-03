using DG.Tweening;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public float duration = 0.2f;

    void OnEnable()
    {
        transform.localScale = Vector3.zero;

        transform.DOScale(Vector3.one, duration)
                 .SetEase(Ease.InOutQuad).SetUpdate(true);
    }

}
