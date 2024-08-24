// Refactor 24.08.24
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// Chapters sahnesindeki b�l�mler aras�ndaki ge�i�i sa�layan swipe'leri y�netmekten sorumludur
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
    /// Sonraki b�l�me ge�er
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
    /// �nceki b�l�me ge�er
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
    /// Sayfay� hareket ettirir
    /// </summary>
    void MovePage()
    {
        chapters.DOLocalMove(_targetPos, tweenTime).SetEase(tweenType);
    }
}
