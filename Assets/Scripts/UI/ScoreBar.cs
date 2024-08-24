// Refactor 12.05.24
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Level içindeki skor çubuðu ile ilgili güncellemelerden sorumlu
/// </summary>
public class ScoreBar : MonoBehaviour
{
    [SerializeField] private Image[] _stars;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Slider _scoreSlider;

    private ScoreManager _scoreManager;

    private void Start()
    {
        _scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe(EventType.ScoreUpdated, UpdateBar);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.ScoreUpdated, UpdateBar);
    }

    private void UpdateBar()
    {
        UpdateScore();
        UpdateSlider();
        UpdateStars();
    }

    private void UpdateScore()
    {
        _scoreText.text = _scoreManager.CurrentScore.ToString();
    }

    private void UpdateSlider()
    {
        _scoreSlider.value = ((float)_scoreManager.CurrentScore / _scoreManager.threeStarScore);
    }

    private void UpdateStars()
    {
        for (int i = 0; i < 2; i++)
        {
            if (_scoreSlider.value >= (0.25f * (i + 1)))
            {
                _stars[i].color = Color.yellow;
            }
        }
        if (_scoreSlider.value >= 1f)
        {
            _stars[2].color = Color.yellow;
        }
    }

}
