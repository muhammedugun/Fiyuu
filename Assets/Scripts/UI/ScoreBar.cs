// Refactor 12.05.24
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    [SerializeField] private Image []stars;
    [SerializeField] private Text scoreText;
    [SerializeField] private Slider scoreSlider;

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
        scoreText.text = _scoreManager.CurrentScore.ToString();
    }

    private void UpdateSlider()
    {
        scoreSlider.value = ((float)_scoreManager.CurrentScore / _scoreManager.starScore[2]);
    }

    private void UpdateStars()
    {
        for (int i = 0; i < 3; i++)
        {
            if (scoreSlider.value >= (0.25f * (i + 1)))
            {
                stars[i].color = Color.yellow;
            }
        }
    }

}
