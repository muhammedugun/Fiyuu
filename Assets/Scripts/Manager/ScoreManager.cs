// Refactor 23.08.24
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Skor bilgisini yönetmekten sorumludur
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static int enemyScore = 500;

    public int threeStarScore;

    [SerializeField] private bool _isFirstLevel;

    private AmmoManager _ammoManager;
    private int _currentScore;
    
    public int CurrentScore
    {
        get { return _currentScore; }
        set
        {
            _currentScore = value;
            EventBus.Publish(EventType.ScoreUpdated);
        }
    } 

    private void Start()
    {
        _ammoManager = FindObjectOfType<AmmoManager>();
    }

    private void OnEnable()
    {
        EventBus<float, BuildingMatter>.Subscribe(EventType.BuildSmashed, CalculateBuildingScore);
        EventBus.Subscribe(EventType.EnemyDied, CalculateEnemyScore);
        if(!_isFirstLevel)
            EventBus.Subscribe(EventType.AllEnemiesDead, CalculateGameEndScore);
        EventBus.Subscribe(EventType.EnOfLevelPopUpOpened, SaveScore);
    }

    private void OnDisable()
    {
        EventBus<float, BuildingMatter>.Unsubscribe(EventType.BuildSmashed, CalculateBuildingScore);
        EventBus.Unsubscribe(EventType.EnemyDied, CalculateEnemyScore);
        if (!_isFirstLevel)
            EventBus.Unsubscribe(EventType.AllEnemiesDead, CalculateGameEndScore);
        EventBus.Unsubscribe(EventType.EnOfLevelPopUpOpened, SaveScore);
    }

    /// <summary>
    /// Kýrýlan herhangi bir objenin verilen parametrelerine bakarak kazanýlacak skoru hesaplar
    /// </summary>
    /// <param name="volumeSize"></param>
    /// <param name="buildingArmor"></param>
    /// <returns></returns>
    public static int CalculateScore(float volumeSize, BuildingMatter buildingArmor)
    {
        int score = (int)(volumeSize * 200) * (int)buildingArmor;
        return score;
    }

    /// <summary>
    /// Kýrýlan binanýn verilen parametrelerine bakarak kazanýlacak skoru hesaplar 
    /// </summary>
    /// <param name="volumeSize"></param>
    /// <param name="buildingArmor"></param>
    private void CalculateBuildingScore(float volumeSize, BuildingMatter buildingArmor)
    {
        CurrentScore += CalculateScore(volumeSize, buildingArmor);
    }

    /// <summary>
    /// Yokedilen düþmanýn skorunu hesaplar
    /// </summary>
    private void CalculateEnemyScore()
    {
        CurrentScore += enemyScore;
    }

    /// <summary>
    /// Level sonu için skoru hesaplayýp günceller
    /// </summary>
    private void CalculateGameEndScore()
    {
        int firedCount = _ammoManager.levelAmmoCount - _ammoManager.ammunition.Count;
        int multiplierScore = _ammoManager.levelAmmoCount / firedCount;
        if (firedCount != 0 && multiplierScore > 1)
        {
            CurrentScore += (int)Math.Round(multiplierScore / 1.0) * 200;
        }
    }

    /// <summary>
    /// Mevcut skor rekor skordan yüksekse mevcut skoru levelin üzerine kalýcý olarak kaydeder
    /// </summary>
    private void SaveScore()
    {
        int levelIndex = int.Parse(SceneManager.GetActiveScene().name.Substring(5));
        if (CurrentScore > (PlayerPrefs.GetInt("LevelScore" + levelIndex)))
        {
            PlayerPrefs.SetInt("LevelScore" + levelIndex, CurrentScore);
            PlayerPrefs.SetInt("LevelStars" + levelIndex, GetRewardedStarCount());
        }
    }

    /// <returns>Kazanýlan yýldýz sayýsý</returns>
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
