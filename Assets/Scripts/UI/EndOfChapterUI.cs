using UnityEngine;
using UnityEngine.SceneManagement;


public class EndOfChapterUI : MonoBehaviour
{
    private ScoreManager _scoreManager;

    private EnemyCountManager _enemyCountManager;
    [SerializeField] private GameObject[] stars;
    [Tooltip("B�l�m sonundaki ��kan y�ld�zlar i�in gerekli skorlar")]
    [SerializeField] private int[] starScore;
    [SerializeField] private GameObject endOfLevelPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject failPanel;
    [SerializeField] private GameObject collisionIcon;
    [SerializeField] private GameObject forwardButton;

    /// <summary>
    /// B�l�m ge�ildi mi?
    /// </summary>
    internal bool isLevelPassed;
    /// <summary>
    /// kazan�lan y�ld�z say�s�
    /// </summary>
    private int _rewardedStarCount;

    private void Start()
    {
        _enemyCountManager = FindObjectOfType<EnemyCountManager>();
        _scoreManager = FindObjectOfType<ScoreManager>();
    }
    private void OnEnable()
    {
        EventBus.Subscribe(EventType.AllObjectsStopped, SetPanelActiveInvoke);
        EventBus.Subscribe(EventType.AllEnemiesDead, SetForwardButton);
        EventBus.Subscribe(EventType.AllEnemiesDead, SetActiveStars);
        EventBus.Subscribe(EventType.AllEnemiesDead, LevelPassControl);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.AllObjectsStopped, SetPanelActiveInvoke);
        EventBus.Unsubscribe(EventType.AllEnemiesDead, SetForwardButton);
        EventBus.Unsubscribe(EventType.AllEnemiesDead, SetActiveStars);
        EventBus.Unsubscribe(EventType.AllEnemiesDead, LevelPassControl);
    }

    
    /// <summary>
    /// Skora g�re b�l�m�n ge�ilip ge�ilmedi�ini kontrol eder.
    /// </summary>
    void LevelPassControl()
    {
        if (_scoreManager.currentScore > starScore[0])
        {
            isLevelPassed = true;
        }
    }

    
    /// <summary>
    /// Skora g�re aktif y�ld�zlar� ayarlar.
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
    
    void SetForwardButton()
    {
        forwardButton.SetActive(true);
    }

    public void OnClickForwardButton()
    {
        SetPanelActive();
    }

    void SetPanelActiveInvoke()
    {
        Invoke(nameof(SetPanelActive), 3f);
    }
    /// <summary>
    /// B�l�m sonu panelini aktif hale getirir.
    /// </summary>
    void SetPanelActive()
    {
        EventBus.Publish(EventType.EndOfChapterUIOpened);
        Time.timeScale = 0f;
        collisionIcon.SetActive(false);
        endOfLevelPanel.SetActive(true);
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
    /// Ana men� sahnesini y�kler.
    /// </summary>
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    /// <summary>
    /// Bir sonraki b�l�m sahnesini y�kler.
    /// </summary>
    public void GoToNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
