using UnityEngine;
using Zenject;
using TMPro;
using UnityEngine.SceneManagement;


public class EndOfChapterUI : MonoBehaviour
{
    [Inject] private InLevelManager _inLevelManager;
    [SerializeField] private GameObject[] stars;
    [Tooltip("Bölüm sonundaki çýkan yýldýzlar için gerekli skorlar")]
    [SerializeField] private int[] starScore;
    [SerializeField] private GameObject endOfLevelPanel;

    [Tooltip("Her 3 yýldýz için kazanýlan tecrübe puaný")]
    [SerializeField] private int[] rewardXP;
    [Tooltip("Her 3 yýldýz için kazanýlan altýn miktarý")]
    [SerializeField] private int[] rewardGold;

    [SerializeField] private TextMeshProUGUI rewardXPText, rewardGoldText;
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
    /// Skora göre bölümün geçilip geçilmediðini kontrol eder.
    /// </summary>
    void LevelPassControl()
    {
        if (_inLevelManager.score > starScore[0])
        {
            isLevelPassed = true;
        }
    }

    /// <summary>
    /// Skora göre aktif yýldýzlarý ayarlar.
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
    /// Bölüm geçme durumuna ve yýldýz sayýsýna göre ödülleri ayarlar.
    /// </summary>
    void SetRewards()
    {
        int rewardXPAmount = isLevelPassed ? rewardXP[_rewardedStarCount - 1] : 0;
        int rewardGoldAmount = isLevelPassed ? rewardGold[_rewardedStarCount - 1] : 0;

        rewardXPText.text = $"XP: {rewardXPAmount}";
        rewardGoldText.text = $"Gold: {rewardGoldAmount}";
    }

    /// <summary>
    /// Bölüm sonu panelini aktif hale getirir.
    /// </summary>
    void SetPanelActive()
    {
        endOfLevelPanel.SetActive(true);
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
