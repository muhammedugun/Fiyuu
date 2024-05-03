using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private int pageCount;
    

    int currentPage;
    Vector3 targetPos;
    public Vector3 pageStep;
    public RectTransform chapters;

    public float tweenTime;
    public Ease tweenType;

    private void Awake()
    {
        currentPage = 1;
        targetPos = chapters.localPosition;
        
    }

    public void Next()
    {
        if(currentPage<pageCount)
        {
            Debug.Log("Next");
            currentPage++;
            titleText.text = "Chapter " + currentPage;
            targetPos += pageStep;
            MovePage();

        }
    }
    public void Previous()
    {
        if(currentPage>1)
        {
            Debug.Log("Previous");
            currentPage--;
            titleText.text = "Chapter " + currentPage;
            targetPos -= pageStep;
            MovePage();
        }
    }
    void MovePage()
    {
        chapters.DOLocalMove(targetPos, tweenTime).SetEase(tweenType);
    }
}
