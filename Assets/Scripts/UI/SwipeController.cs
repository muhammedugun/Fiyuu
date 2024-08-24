// Refactor 24.08.24
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// Chapters sahnesindeki bölümler arasýndaki geçiþi saðlayan swipe'leri yönetmekten sorumludur
/// </summary>
public class SwipeController : MonoBehaviour
{
    public Vector3 pageStep;
    public RectTransform chapters;
    public Ease tweenType;
    public float tweenTime;

    [SerializeField] private Text _titleText;
    [SerializeField] private int _pageCount;

    private int _currentPage;
    private Vector3 _targetPos;

    private void Awake()
    {
        _currentPage = 1;
        _targetPos = chapters.localPosition;
    }

    /// <summary>
    /// Sonraki bölüme geçer
    /// </summary>
    public void Next()
    {
        if(_currentPage<_pageCount)
        {
            _currentPage++;
            _titleText.text = "Chapter " + _currentPage;
            _targetPos += pageStep;
            MovePage();
        }
    }

    /// <summary>
    /// Önceki bölüme geçer
    /// </summary>
    public void Previous()
    {
        if(_currentPage>1)
        {
            _currentPage--;
            _titleText.text = "Chapter " + _currentPage;
            _targetPos -= pageStep;
            MovePage();
        }
    }

    /// <summary>
    /// Sayfayý hareket ettirir
    /// </summary>
    void MovePage()
    {
        chapters.DOLocalMove(_targetPos, tweenTime).SetEase(tweenType);
    }
}
