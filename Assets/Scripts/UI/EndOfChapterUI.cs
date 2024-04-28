using UnityEngine;
using UnityEngine.SceneManagement;


public class EndOfChapterUI : MonoBehaviour
{
    private ScoreManager _scoreManager;

    private EnemyCountManager _enemyCountManager;
    [SerializeField] private GameObject[] stars;
    [Tooltip("Bölüm sonundaki çýkan yýldýzlar için gerekli skorlar")]
    [SerializeField] private int[] starScore;
    [SerializeField] private GameObject endOfLevelPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject failPanel;

    /// <summary>
    /// Bölüm geçildi mi?
    /// </summary>
    internal bool isLevelPassed;
    /// <summary>
    /// kazanýlan yýldýz sayýsý
    /// </summary>
    private int _rewardedStarCount;

    private void Start()
    {
        _enemyCountManager = FindObjectOfType<EnemyCountManager>();
        _scoreManager = FindObjectOfType<ScoreManager>();
    }
    private void OnEnable()
    {
        EventBus.Subscribe(EventType.GameOver, SetPanelActive);
        EventBus.Subscribe(EventType.GameOver, SetActiveStars);
        EventBus.Subscribe(EventType.GameOver, LevelPassControl);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.GameOver, SetPanelActive);
        EventBus.Unsubscribe(EventType.GameOver, SetActiveStars);
        EventBus.Unsubscribe(EventType.GameOver, LevelPassControl);
    }

    
    /// <summary>
    /// Skora göre bölümün geçilip geçilmediðini kontrol eder.
    /// </summary>
    void LevelPassControl()
    {
        if (_scoreManager.currentScore > starScore[0])
        {
            isLevelPassed = true;
        }
    }

    
    /// <summary>
    /// Skora göre aktif yýldýzlarý ayarlar.
    /// </summary>
    void SetActiveStars()
    {
        if(stars.Length>0)
        {
            for (int i = 0; i < starScore.Length; i++)
            {
                if (_scoreManager.currentScore > starScore[i])
                {
                    _rewardedStarCount++;
                }
            }
            for (int i = 0; i < _rewardedStarCount; i++)
            {
                stars[i]?.SetActive(true);
            }
        }
        
    }
    

    /// <summary>
    /// Bölüm sonu panelini aktif hale getirir.
    /// </summary>
    void SetPanelActive()
    {
        endOfLevelPanel.SetActive(true);
        Debug.Log(_enemyCountManager._enemyCount);
        if(_enemyCountManager._enemyCount<=0)
        {
            winPanel.SetActive(true);
        }
        else
        {
            failPanel.SetActive(true);
        }

    }

    /// <summary>
    /// Ana menü sahnesini yükler.
    /// </summary>
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    /// <summary>
    /// Bir sonraki bölüm sahnesini yükler.
    /// </summary>
    public void GoToNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
