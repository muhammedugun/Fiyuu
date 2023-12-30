using UnityEngine;
using Zenject;
using TMPro;

public class EndOfLevel : MonoBehaviour
{
    [Inject] InLevelManager inLevelManager;
    [SerializeField] GameObject[] stars;
    [Tooltip("B�l�m sonundaki ��kan y�ld�zlar i�in gerekli skorlar")]
    [SerializeField] private int[] starScore;
    [SerializeField] GameObject endOfLevelPanel;

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
    private int rewardedStarCount;

    private void Start()
    {
        Subscribe();
    }

    void Subscribe()
    {
        inLevelManager.OnGameOver += SetActiveStars;
        inLevelManager.OnGameOver += SetPanelActive;
        inLevelManager.OnGameOver += LevelPassControl;
        inLevelManager.OnGameOver += SetRewards;
    }

    void LevelPassControl()
    {
        if (inLevelManager.score > starScore[0])
        {
            isLevelPassed = true;
        }
    }

    void SetActiveStars()
    {
        
        for (int i = 0; i < starScore.Length; i++)
        {
            if(inLevelManager.score > starScore[i])
            {
                rewardedStarCount++;
            }
        }
        for (int i = 0; i < rewardedStarCount; i++)
        {
            stars[i].SetActive(true);
        }
    }

    void SetRewards()
    {
        if(isLevelPassed)
        {
            rewardXPText.text = "XP: " + rewardXP[rewardedStarCount - 1];
            rewardGoldText.text = "Gold: " + rewardGold[rewardedStarCount - 1];
        }
        else
        {
            rewardXPText.text = "XP: " + 0;
            rewardGoldText.text = "Gold: " + 0;
        }
        
    }

    void SetPanelActive()
    {
        endOfLevelPanel.SetActive(true);
    }
}
