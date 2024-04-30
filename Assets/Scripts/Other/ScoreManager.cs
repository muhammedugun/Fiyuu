using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private AmmoManager _ammoManager;

    [Header("UI")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Slider scoreSlider;
    [SerializeField] private GameObject stars;
    [Header("EndOfLevel")]
    [SerializeField] private Text winScoreText;
    [SerializeField] private List<Image> winStars;


    [SerializeField] private int levelScore;
    [HideInInspector] public int currentScore;
    

    private void Start()
    {
        _ammoManager = FindObjectOfType<AmmoManager>();
    }

    private void OnEnable()
    {

        EventBus<float, BuildingMatter>.Subscribe(EventType.BuildSmashed, CalculateBuildingScore);
        EventBus.Subscribe(EventType.BuildSmashed, UpdateScoreOnGUI);

        EventBus.Subscribe(EventType.EnemyDied, CalculateEnemyScore);
        EventBus.Subscribe(EventType.EnemyDied, UpdateScoreOnGUI);

        EventBus.Subscribe(EventType.LevelEnd, CalculateGameEndScore);
        EventBus.Subscribe(EventType.LevelEnd, UpdateWinPopUp);

    }

    private void OnDisable()
    {
        EventBus<float, BuildingMatter>.Unsubscribe(EventType.BuildSmashed, CalculateBuildingScore);
        EventBus.Unsubscribe(EventType.BuildSmashed, UpdateScoreOnGUI);

        EventBus.Unsubscribe(EventType.EnemyDied, CalculateEnemyScore);
        EventBus.Unsubscribe(EventType.EnemyDied, UpdateScoreOnGUI);

        EventBus.Unsubscribe(EventType.LevelEnd, CalculateGameEndScore);
        EventBus.Unsubscribe(EventType.LevelEnd, UpdateWinPopUp);
    }

    /// <summary>
    /// GUI'daki skoru güncelle
    /// </summary>
    private void UpdateScoreOnGUI()
    {
        scoreText.text = "Score: " + currentScore;
        UpdateScoreBar();
    }

    /*
     Skor nasýl hesaplanacak?: Yýkýlan bina + Kullanýlan mühimattýn azlýðý
     Yýkýlan binanýn vereceði skor: Binanýn boyutu*Binanýn malzemesi(zýrhý)
     Mühimmat azlýðýnýn vereceði skor: (Sabit bir deðer) * (Mühimmat sýnýrý / kullanýlan mühimat) -> mühimmatýn tamamý kullanýldýysa mühimmat azlýðý sýfýr sayýlýr
     */
    void CalculateBuildingScore(float volumeSize, BuildingMatter buildingArmor)
    {
        currentScore += (int)(volumeSize * 50) * (int)buildingArmor;

    }

    /// <summary>
    /// Enemy objesi için skor hesaplamasý yapar
    /// </summary>
    void CalculateEnemyScore()
    {
        currentScore += 200;
    }

    /// <summary>
    /// Oyun sonu için skor hesaplamasý yapar
    /// </summary>
    void CalculateGameEndScore()
    {
        int firedCount = _ammoManager.levelAmmoCount - _ammoManager.ammunition.Count;
        if (firedCount != 0 && (_ammoManager.ammunition.Count / firedCount) > 1)
        {
            currentScore += 100 * _ammoManager.ammunition.Count / firedCount;
        }
        int levelIndex = int.Parse(SceneManager.GetActiveScene().name.Substring(5));
        if (levelIndex > (PlayerPrefs.GetInt("LevelScore" + levelIndex)))
        {
            PlayerPrefs.SetInt("LevelScore" + levelIndex, currentScore);
        }
        UpdateScoreBar();
    }

    public static int CalculateScore(float volumeSize, BuildingMatter buildingArmor)
    {
        int score = (int)(volumeSize * 50) * (int)buildingArmor;
        return score;
    }


    internal void UpdateScoreBar()
    {

        scoreSlider.value = ((float)currentScore / levelScore);
        if (scoreSlider.value >= 0.25f)
        {
            stars.transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
            stars.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.yellow;
        }
        if (scoreSlider.value >= 0.50f)
        {
            stars.transform.GetChild(1).GetComponent<Image>().color = Color.yellow;
            stars.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.yellow;
        }
        if (scoreSlider.value >= 0.75f)
        {
            stars.transform.GetChild(2).GetComponent<Image>().color = Color.yellow;
            stars.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = Color.yellow;
        }

    }

    /// <summary>
    /// Bölüm sonundaki win ekranýndaki skoru günceller
    /// </summary>
    private void UpdateWinPopUp()
    {
        winScoreText.text = currentScore.ToString();

        int levelIndex = int.Parse(SceneManager.GetActiveScene().name.Substring(5));

        if (scoreSlider.value>=0.75f)
        {
            PlayerPrefs.SetInt("LevelStars" + levelIndex, 3);
        }
        else if (scoreSlider.value >= 0.5f)
        {
            PlayerPrefs.SetInt("LevelStars" + levelIndex, 2);
        }
        else if (scoreSlider.value >= 0.25f)
        {
            PlayerPrefs.SetInt("LevelStars" + levelIndex, 1);
        }
        else
        {
            PlayerPrefs.SetInt("LevelStars" + levelIndex, 0);
        }

        UpdateWinStar(0.25f, winStars[0]);
        UpdateWinStar(0.25f, winStars[0].transform.GetChild(0).GetComponent<Image>());

        UpdateWinStar(0.50f, winStars[1]);
        UpdateWinStar(0.50f, winStars[1].transform.GetChild(0).GetComponent<Image>());

        UpdateWinStar(0.75f, winStars[2]);
        UpdateWinStar(0.75f, winStars[2].transform.GetChild(0).GetComponent<Image>());

    }

    private void UpdateWinStar(float condition, Image image)
    {
        if (scoreSlider.value >= condition)
        {
            image.DOFade(1f, 2f).SetEase(Ease.InBounce);
        }
    }

}
