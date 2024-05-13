// Refactor 12.05.24
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public int threeStarScore;

    public static int enemyScore = 500;

    private int currentScore;
    [SerializeField] private bool isFirstLevel;
    public int CurrentScore
    {
        get { return currentScore; }
        set
        {
            currentScore = value;
            EventBus.Publish(EventType.ScoreUpdated);
        }
    }


    private AmmoManager _ammoManager;


    private void Start()
    {
        _ammoManager = FindObjectOfType<AmmoManager>();
    }

    private void OnEnable()
    {
        EventBus<float, BuildingMatter>.Subscribe(EventType.BuildSmashed, CalculateBuildingScore);
        EventBus.Subscribe(EventType.EnemyDied, CalculateEnemyScore);
        if(!isFirstLevel)
            EventBus.Subscribe(EventType.AllEnemiesDead, CalculateGameEndScore);
        EventBus.Subscribe(EventType.EnOfLevelPopUpOpened, SaveScore);
    }

    private void OnDisable()
    {
        EventBus<float, BuildingMatter>.Unsubscribe(EventType.BuildSmashed, CalculateBuildingScore);
        EventBus.Unsubscribe(EventType.EnemyDied, CalculateEnemyScore);
        if (!isFirstLevel)
            EventBus.Unsubscribe(EventType.AllEnemiesDead, CalculateGameEndScore);
        EventBus.Unsubscribe(EventType.EnOfLevelPopUpOpened, SaveScore);
    }

    public static int CalculateScore(float volumeSize, BuildingMatter buildingArmor)
    {
        int score = (int)(volumeSize * 200) * (int)buildingArmor;
        return score;
    }

    private void CalculateBuildingScore(float volumeSize, BuildingMatter buildingArmor)
    {
        CurrentScore += CalculateScore(volumeSize, buildingArmor);
    }

    private void CalculateEnemyScore()
    {
        CurrentScore += enemyScore;
    }

    private void CalculateGameEndScore()
    {
        int firedCount = _ammoManager.levelAmmoCount - _ammoManager.ammunition.Count;
        int multiplierScore = _ammoManager.levelAmmoCount / firedCount;
        if (firedCount != 0 && multiplierScore > 1)
        {
            CurrentScore += (int)Math.Round(multiplierScore / 1.0) * 200;
        }
    }

    private void SaveScore()
    {
        int levelIndex = int.Parse(SceneManager.GetActiveScene().name.Substring(5));
        if (CurrentScore > (PlayerPrefs.GetInt("LevelScore" + levelIndex)))
        {
            PlayerPrefs.SetInt("LevelScore" + levelIndex, CurrentScore);
            PlayerPrefs.SetInt("LevelStars" + levelIndex, GetRewardedStarCount());
        }
    }

    public int GetRewardedStarCount()
    {
        int rewardedStarCount = 0;
        float scoreBarSlider = ((float)CurrentScore / threeStarScore);
        for (int i = 0; i < 2; i++)
        {
            if (scoreBarSlider >= (0.25f * (i + 1)))
            {
                rewardedStarCount++;
            }
        }
        if (scoreBarSlider >= 1f)
        {
            rewardedStarCount++;
        }

        return rewardedStarCount;
    }

   
}
