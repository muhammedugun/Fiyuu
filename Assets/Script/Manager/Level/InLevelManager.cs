using System;
using UnityEngine;
using Zenject;
using TMPro;

public class InLevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ammoCountText;
    /// <summary>
    /// Leveldeki mühimmat sýnýrý
    /// </summary>
    public int ammoLimit;
    /// <summary>
    /// Ateþlenmiþ mühimmat sayýsý
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
     Skor nasýl hesaplanacak?: Yýkýlan bina + Kullanýlan mühimattýn azlýðý
     Yýkýlan binanýn vereceði skor: Binanýn boyutu*Binanýn malzemesi(zýrhý)
     Mühimmat azlýðýnýn vereceði skor: (Sabit bir deðer) * (Mühimmat sýnýrý / kullanýlan mühimat) -> mühimmatýn tamamý kullanýldýysa mühimmat azlýðý sýfýr sayýlýr
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
