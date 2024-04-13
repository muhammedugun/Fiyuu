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
        if(scoreFill!=null)
            scoreFill.fillAmount = ((float)inlevelManager.score / inlevelManager.finalScore);
        if (scoreFill.fillAmount >= 0.3f)
        {
            if(scoreIcons[0]!=null)
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
