using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject InLevelPopUps;
    [SerializeField] private GameObject areYouSure;
    [SerializeField] private GameObject collisionIcon;

    [SerializeField] private Image soundButtonImage;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    private void Start()
    {
        if (AudioListener.volume == 0f)
        {
            soundButtonImage.sprite = soundOffSprite;
        }
        else
        {
            soundButtonImage.sprite = soundOnSprite;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClickPauseButton()
    {
        collisionIcon.SetActive(false);
        InLevelPopUps.SetActive(true);
        InLevelPopUps.transform.Find("Pause").gameObject.SetActive(true);
        PauseGame();
    }

    public void OnClickResumeButton()
    {
        collisionIcon.SetActive(true);
        ResumeGame();
        var rectTransform = InLevelPopUps.transform.Find("Pause").GetComponent<RectTransform>();

        rectTransform.localScale = Vector3.one;

        rectTransform.DOScale(Vector3.zero, 0.2f)
                 .SetEase(Ease.InOutQuad).OnComplete(() =>
                 {
                     InLevelPopUps.transform.Find("Pause").gameObject.SetActive(false);
                     InLevelPopUps.SetActive(false);
                 });
    }

    public void OnClickRestartButton()
    {
        ResumeGame();
        RestartLevel();
    }

    public void OpenAreYouSure()
    {
        areYouSure.SetActive(true);
    }

    public void CloseAreYouSure()
    {
        areYouSure.SetActive(false);
    }

    public void OpenMainMenu()
    {
        ResumeGame();
        Infrastructure.LoadScene("MainMenu");
    }

    public void OpenNextLevel()
    {
        ResumeGame();
        string thisSceneName = SceneManager.GetActiveScene().name;
        int levelNumber = int.Parse(thisSceneName.Substring(5, thisSceneName.Length - 5));
        Infrastructure.LoadScene("Level"+(levelNumber+1));

    }

    public void SoundButton()
    {
        if(AudioListener.volume!=0f)
        {
            AudioListener.volume = 0f;
            soundButtonImage.sprite = soundOffSprite;
        }
        else
        {
            AudioListener.volume = 1f;
            soundButtonImage.sprite = soundOnSprite;
        }
        
    }

}
