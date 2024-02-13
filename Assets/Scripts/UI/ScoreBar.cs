using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    [SerializeField] private List<Image> scoreIcons;
    [SerializeField] private InLevelManager inlevelManager;
    [SerializeField] private Image scoreFill;
    private void Start()
    {
        UpdateScoreIcons();
    }
    internal void UpdateScoreIcons()
    {
        Debug.Log("score " + inlevelManager.score);
        Debug.Log("finalScore " + inlevelManager.finalScore);
        scoreFill.fillAmount = ((float)inlevelManager.score / inlevelManager.finalScore);
        if (scoreFill.fillAmount >= 0.3f)
        {
            scoreIcons[0].color = Color.yellow;
        }
        if (scoreFill.fillAmount >= 0.66f)
        {
            scoreIcons[1].color = Color.yellow;
        }
        if (scoreFill.fillAmount >= 1f)
        {
            scoreIcons[2].color = Color.yellow;
        }
        
    }
    
}
