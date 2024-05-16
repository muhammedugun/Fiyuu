using DG.Tweening;
using UnityEngine;

public class BuildingFragment : MonoBehaviour
{
    
    public float scaleDuration=1f;
    public float InvokeDelay=2f;

    private void Start()
    {

        InvokeDelay = Random.Range(InvokeDelay*0.5f, InvokeDelay * 1.5f);
        Invoke(nameof(SetScale), InvokeDelay);
    }


    void SetScale()
    {
        transform.DOScale(new Vector3(0f, 0f, 0f), scaleDuration).SetEase(Ease.InOutCubic).OnComplete(()=>
        {
            Destroy(this.gameObject);
        }
        );
    }
}

