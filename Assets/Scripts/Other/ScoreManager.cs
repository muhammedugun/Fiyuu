using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private AmmoManager _ammoManager;

    [Header("GUI")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Image scoreFill;
    [SerializeField] private Image[] scoreIcons;

    [HideInInspector] public int currentScore;
    [HideInInspector] public int levelScore;

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

        EventBus.Subscribe(EventType.GameOver, CalculateGameEndScore);
        
    }

    private void OnDisable()
    {
        EventBus<float, BuildingMatter>.Unsubscribe(EventType.BuildSmashed, CalculateBuildingScore);
        EventBus.Unsubscribe(EventType.BuildSmashed, UpdateScoreOnGUI);

        EventBus.Unsubscribe(EventType.EnemyDied, CalculateEnemyScore);
        EventBus.Unsubscribe(EventType.EnemyDied, UpdateScoreOnGUI);

        EventBus.Unsubscribe(EventType.GameOver, CalculateGameEndScore);
    }

    /// <summary>
    /// GUI'daki skoru g�ncelle
    /// </summary>
    private void UpdateScoreOnGUI()
    {
        scoreText.text = "Score: " + currentScore;
        //UpdateScoreBar();
    }

    /*
     Skor nas�l hesaplanacak?: Y�k�lan bina + Kullan�lan m�himatt�n azl���
     Y�k�lan binan�n verece�i skor: Binan�n boyutu*Binan�n malzemesi(z�rh�)
     M�himmat azl���n�n verece�i skor: (Sabit bir de�er) * (M�himmat s�n�r� / kullan�lan m�himat) -> m�himmat�n tamam� kullan�ld�ysa m�himmat azl��� s�f�r say�l�r
     */
    void CalculateBuildingScore(float volumeSize, BuildingMatter buildingArmor)
    {
        currentScore += (int)(volumeSize * 50) * (int)buildingArmor;
    }

    /// <summary>
    /// Enemy objesi i�in skor hesaplamas� yapar
    /// </summary>
    void CalculateEnemyScore()
    {
        currentScore += 200;
    }

    /// <summary>
    /// Oyun sonu i�in skor hesaplamas� yapar
    /// </summary>
    void CalculateGameEndScore()
    {
        int firedCount = _ammoManager.levelAmmoCount - _ammoManager.ammunition.Count;
        if (firedCount != 0 && (_ammoManager.ammunition.Count / firedCount) > 1)
        {
            currentScore += 100 * _ammoManager.ammunition.Count / firedCount;
        }
    }

    public static int CalculateScore(float volumeSize, BuildingMatter buildingArmor)
    {
        int score = (int)(volumeSize * 50) * (int)buildingArmor;
        return score;
    }


    internal void UpdateScoreBar()
    {
        if (scoreFill != null)
            scoreFill.fillAmount = ((float)currentScore / levelScore);
        if (scoreFill.fillAmount >= 0.3f)
        {
            if (scoreIcons[0] != null)
                scoreIcons[0].color = Color.yellow;
        }
        if (scoreFill.fillAmount >= 0.66f)
        {
            if (scoreIcons[1] != null)
                scoreIcons[1].color = Color.yellow;
        }
        if (scoreFill.fillAmount >= 1f)
        {
            if (scoreIcons[2] != null)
                scoreIcons[2].color = Color.yellow;
        }

    }

}
