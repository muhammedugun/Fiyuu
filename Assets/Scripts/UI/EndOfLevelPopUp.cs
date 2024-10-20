// Refactor 23.08.24
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Level sonu popup�n� y�netmekten sorumludur
/// </summary>
public class EndOfLevelPopUp : MonoBehaviour
{
    [SerializeField] private Image[] _stars;
    [SerializeField] private GameObject _popUp;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _failPanel;
    [SerializeField] private GameObject _ForwardButton;
    [SerializeField] private Text _scoreText;

    private ScoreManager _scoreManager;
    private EnemyCountManager _enemyCountManager;

    private void Start()
    {
        _scoreManager = FindObjectOfType<ScoreManager>();
        _enemyCountManager = FindObjectOfType<EnemyCountManager>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe(EventType.AllEnemiesDead, OpenForwardButton);
        EventBus.Subscribe(EventType.AllObjectsStopped, OpenPopUpInvoke);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.AllEnemiesDead, OpenForwardButton);
        EventBus.Unsubscribe(EventType.AllObjectsStopped, OpenPopUpInvoke);
    }

    /// <summary>
    /// Next Level butonunu �al��t�r�r. Sonraki levele ge�i� yapar.
    /// </summary>
    public void OnClickNextLevel()
    {
        GameManager.ResumeLevel();
        string thisSceneName = SceneManager.GetActiveScene().name;
        int levelNumber = int.Parse(thisSceneName.Substring(5, thisSceneName.Length - 5));
        if (IsSceneInBuildSettings("Level" + (levelNumber + 1)))
        {
            Infrastructure.LoadScene("Level" + (levelNumber + 1));
        }
        else
        {
            Infrastructure.LoadScene("DemoEnd");
        }

    }

    /// <summary>
    /// Sahnenin edit�run build settings b�l�m�nde ayarl� olup olmad���n� kontrol eder
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private bool IsSceneInBuildSettings(string name)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneName == name)
            {
                return true;
            }
        }
        return false;
    }

    public void OnClickMainMenu()
    {
        GameManager.ResumeLevel();
        Infrastructure.LoadScene("MainMenu");
    }

    public void OnClickRestartButton()
    {
        GameManager.RestartLevel();
    }

    /// <summary>
    /// Popup �zerindeki aktif ve deaktif olacak y�ld�zlar� ayarlar
    /// </summary>
    private void SetStars()
    {
        if (_stars.Length > 0)
        {
            int rewardedStarCount = _scoreManager.GetRewardedStarCount();

            for (int i = 0; i < rewardedStarCount; i++)
            {
                _stars[i]?.gameObject.SetActive(true);
                _stars[i]?.DOFade(1f, 2f).SetEase(Ease.InBounce).SetUpdate(true);
                _stars[i].transform.GetChild(0).GetComponent<Image>().DOFade(1f, 1f).SetEase(Ease.InBounce).SetUpdate(true);
            }
        }
    }

    /// <summary>
    /// Popup� gecikmeli bir �ekilde a�ar
    /// </summary>
    private void OpenPopUpInvoke()
    {
        Invoke(nameof(OpenPopUp), 3f);
    }

    public void OpenPopUp()
    {
        EventBus.Publish(EventType.EnOfLevelPopUpOpened);
        _popUp.SetActive(true);
        SetWinOrFailPanel();
        GameManager.PauseLevel();
    }

    /// <summary>
    /// Kazanma ve kaybetme durumuna g�re uygun olan popup� a�ar
    /// </summary>
    private void SetWinOrFailPanel()
    {
        if (_enemyCountManager._enemyCount <= 0)
        {
            _winPanel.SetActive(true);
            _scoreText.text = _scoreManager.CurrentScore.ToString();
            SetStars();
        }
        else
        {
            _failPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Forward, yani h�zl�ca ge�ip level sonu popup�n�n a��lmas�n� sa�layan butonu �al��t�r�r.
    /// </summary>
    private void OpenForwardButton()
    {
        if (!InLevelManager.isAllObjectsStopped)
        {
            _ForwardButton.SetActive(true);
        }
    }

}
