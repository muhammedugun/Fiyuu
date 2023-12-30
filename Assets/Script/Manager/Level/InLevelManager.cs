using System;
using UnityEngine;
using Zenject;
using TMPro;

public class InLevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ammoCountText;
    /// <summary>
    /// Leveldeki m�himmat s�n�r�
    /// </summary>
    public int ammoLimit;
    /// <summary>
    /// Ate�lenmi� m�himmat say�s�
    /// </summary>
    private int _firedAmmoCount;
    [Inject] Catapult catapult;
    //[Inject] Trebuchet trebuchet;
    
    [SerializeField] private TextMeshProUGUI _scoreText;
    internal int score;
    /// <summary>
    /// Oyun bitti mi?
    /// </summary>
    private bool _isGameEnd;

    public event Action OnGameOver;
    private void Start()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    void Subscribe()
    {
        if(catapult!=null)
        catapult.OnLaunched += AddWeaponCount;
        //if(trebuchet!=null)
        //trebuchet.OnLaunched += AddWeaponCount;
        OnGameOver += GameOver;
        Building.OnBuildSmashed += CalculateScore;
    }

   
    void UnSubscribe()
    {
        catapult.OnLaunched -= AddWeaponCount;
        //trebuchet.OnLaunched -= AddWeaponCount;
        OnGameOver -= GameOver;
        Building.OnBuildSmashed -= CalculateScore;
    }

    void AddWeaponCount()
    {
        _firedAmmoCount++; 
        _ammoCountText.text= "Ammo Count: " + (ammoLimit - _firedAmmoCount);
        WeaponCountLimitCheck();
    }

    void WeaponCountLimitCheck()
    {
        if(ammoLimit - _firedAmmoCount <= 0)
        {
            OnGameOver?.Invoke();
        }
    }

    void GameOver()
    {
        _isGameEnd = true;
        Debug.LogWarning("Oyun Bitti");
    }

    /*
     Skor nas�l hesaplanacak?: Y�k�lan bina + Kullan�lan m�himatt�n azl���
     Y�k�lan binan�n verece�i skor: Binan�n boyutu*Binan�n malzemesi(z�rh�)
     M�himmat azl���n�n verece�i skor: (Sabit bir de�er) * (M�himmat s�n�r� / kullan�lan m�himat) -> m�himmat�n tamam� kullan�ld�ysa m�himmat azl��� s�f�r say�l�r
     */

    void CalculateScore(float volumeSize, BuildingMatter buildingArmor)
    {
        if(_isGameEnd && (ammoLimit / _firedAmmoCount)>1)
        {
            score += 100 * ammoLimit / _firedAmmoCount;
        }
        Debug.Log("volume size: " + volumeSize);
        score += (int)(volumeSize*10) * (int)buildingArmor;
        _scoreText.text = "Score: " + score;
    }

    

}
