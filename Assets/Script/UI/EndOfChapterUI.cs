using UnityEngine;
using Zenject;
using TMPro;
using UnityEngine.SceneManagement;


public class EndOfChapterUI : MonoBehaviour
{
    [Inject] private InLevelManager _inLevelManager;
    [SerializeField] private GameObject[] stars;
    [Tooltip("B�l�m sonundaki ��kan y�ld�zlar i�in gerekli skorlar")]
    [SerializeField] private int[] starScore;
    [SerializeField] private GameObject endOfLevelPanel;

    [Tooltip("Her 3 y�ld�z i�in kazan�lan tecr�be puan�")]
    [SerializeField] private int[] rewardXP;
    [Tooltip("Her 3 y�ld�z i�in kazan�lan alt�n miktar�")]
    [SerializeField] private int[] rewardGold;

    [SerializeField] private TextMeshProUGUI rewardXPText, rewardGoldText;
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
        Subscribe();
    }

    void Subscribe()
    {
        InLevelManager.OnGameOver += SetPanelActive;
        InLevelManager.OnGameOver += SetActiveStars;
        InLevelManager.OnGameOver += LevelPassControl;
        InLevelManager.OnGameOver += SetRewards;
    }

    /// <summary>
    /// Skora g�re b�l�m�n ge�ilip ge�ilmedi�ini kontrol eder.
    /// </summary>
    void LevelPassControl()
    {
        if (_inLevelManager.score > starScore[0])
        {
            isLevelPassed = true;
        }
    }

    /// <summary>
    /// Skora g�re aktif y�ld�zlar� ayarlar.
    /// </summary>
    void SetActiveStars()
    {
        for (int i = 0; i < starScore.Length; i++)
        {
            if (_inLevelManager.score > starScore[i])
            {
                _rewardedStarCount++;
            }
        }
        for (int i = 0; i < _rewardedStarCount; i++)
        {
            stars[i]?.SetActive(true);
        }
    }

    /// <summary>
    /// B�l�m ge�me durumuna ve y�ld�z say�s�na g�re �d�lleri ayarlar.
    /// </summary>
    void SetRewards()
    {
        int rewardXPAmount = isLevelPassed ? rewardXP[_rewardedStarCount - 1] : 0;
        int rewardGoldAmount = isLevelPassed ? rewardGold[_rewardedStarCount - 1] : 0;

        rewardXPText.text = $"XP: {rewardXPAmount}";
        rewardGoldText.text = $"Gold: {rewardGoldAmount}";
    }

    /// <summary>
    /// B�l�m sonu panelini aktif hale getirir.
    /// </summary>
    void SetPanelActive()
    {
        endOfLevelPanel.SetActive(true);
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
