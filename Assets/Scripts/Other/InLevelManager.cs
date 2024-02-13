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
     Skor nas�l hesaplanacak?: Y�k�lan bina + Kullan�lan m�himatt�n azl���
     Y�k�lan binan�n verece�i skor: Binan�n boyutu*Binan�n malzemesi(z�rh�)
     M�himmat azl���n�n verece�i skor: (Sabit bir de�er) * (M�himmat s�n�r� / kullan�lan m�himat) -> m�himmat�n tamam� kullan�ld�ysa m�himmat azl��� s�f�r say�l�r
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
