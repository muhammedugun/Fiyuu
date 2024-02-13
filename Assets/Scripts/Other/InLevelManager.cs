using System;
using UnityEngine;
using TMPro;

public class InLevelManager : MonoBehaviour
{
    public int finalScore;
    
    internal int score;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private AmmoCountUI ammoCount;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    private int _enemyCount;
    private bool _isGameEnd;
    public static event Action OnGameOver;
    private void Start()
    {
        Subscribe();
        CalculateEnemyCount();
        UpdateEnemyCountText();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    void Subscribe()
    {
        Enemy.OnDied += UpdateEnemyCount;
        Building.OnBuildSmashed += CalculateScore;
        AmmoCountUI.OnOutOfAmmo += GameOver;
    }

   
    void UnSubscribe()
    {
        Enemy.OnDied -= UpdateEnemyCount;
        AmmoCountUI.OnOutOfAmmo -= GameOver;
    }

    private void CalculateEnemyCount()
    {
        _enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    private void UpdateEnemyCount()
    {
        _enemyCount--;
        UpdateEnemyCountText();
        if (_enemyCount<=0)
            GameOver();
    }

    private void UpdateEnemyCountText()
    {
        enemyCountText.text=_enemyCount.ToString();
    }

    void GameOver()
    {
        OnGameOver.Invoke();
        _isGameEnd = true;
        Debug.LogWarning("Oyun Bitti");
    }

    /*
     Skor nasýl hesaplanacak?: Yýkýlan bina + Kullanýlan mühimattýn azlýðý
     Yýkýlan binanýn vereceði skor: Binanýn boyutu*Binanýn malzemesi(zýrhý)
     Mühimmat azlýðýnýn vereceði skor: (Sabit bir deðer) * (Mühimmat sýnýrý / kullanýlan mühimat) -> mühimmatýn tamamý kullanýldýysa mühimmat azlýðý sýfýr sayýlýr
     */

    [SerializeField] private ScoreBar scoreBar;
    void CalculateScore(float volumeSize, BuildingMatter buildingArmor)
    {
        if(_isGameEnd && (ammoCount.limitCount / ammoCount.firedCount) >1)
        {
            score += 100 * ammoCount.limitCount / ammoCount.firedCount;
        }
        Debug.Log("volume size: " + volumeSize);
        score += (int)(volumeSize*10) * (int)buildingArmor;
        scoreText.text = "Score: " + score;
        scoreBar.UpdateScoreIcons();
    }
    
    

}
