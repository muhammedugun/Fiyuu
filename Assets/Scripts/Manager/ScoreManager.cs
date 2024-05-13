// Refactor 12.05.24
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public int[] starScore;

    public static int enemyScore = 500;

    private int currentScore;
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
        EventBus.Subscribe(EventType.EnOfLevelPopUpOpened, CalculateGameEndScore);
    }

    private void OnDisable()
    {
        EventBus<float, BuildingMatter>.Unsubscribe(EventType.BuildSmashed, CalculateBuildingScore);
        EventBus.Unsubscribe(EventType.EnemyDied, CalculateEnemyScore);
        EventBus.Unsubscribe(EventType.EnOfLevelPopUpOpened, CalculateGameEndScore);
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
        for (int i = 0; i < 3; i++)
        {
            if (CurrentScore > starScore[i])
            {
                rewardedStarCount++;
            }
        }
        return rewardedStarCount;
    }

   
}
