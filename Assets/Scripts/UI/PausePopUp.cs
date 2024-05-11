//Refactor 11.05.24

using UnityEngine;
using UnityEngine.UI;

public class PausePopUp : MonoBehaviour
{
    [SerializeField] private Image soundButtonImage;
    [SerializeField] private Sprite soundOnSprite, soundOffSprite;
    [SerializeField] private RectTransform pausePopUpTransform;
    [SerializeField] private GameObject popUps;

    private const string volumeKey = "Volume";

    private void OnEnable()
    {
        UpdateSoundButtonSprite();
    }

    private void UpdateSoundButtonSprite()
    {
        bool isSoundOn = AudioListener.volume != 0f;
        soundButtonImage.sprite = isSoundOn ? soundOnSprite : soundOffSprite;
    }

    public void OnClickSoundButton()
    {
        bool isSoundOn = AudioListener.volume != 0f;
        AudioListener.volume = isSoundOn ? 0f : 1f;
        PlayerPrefs.SetInt(volumeKey, isSoundOn ? 0 : 1);
        UpdateSoundButtonSprite();
    }

    public void OnClickRestartButton()
    {
        GameManager.RestartLevel();
    }

    public void OnClickResumeButton()
    {
        GameManager.ResumeLevel();
        ClosePausePopUp();
    }

    public void OnClickPauseButton()
    {
        GameManager.PauseLevel();
        popUps.SetActive(true);
        pausePopUpTransform.gameObject.SetActive(true);
    }

    public void OnClickMainMenu()
    {
        GameManager.ResumeLevel();
        Infrastructure.LoadScene("MainMenu");
    }

    private void ClosePausePopUp()
    {
        if (pausePopUpTransform != null)
        {
            UIAnimation.OpenPopUp(pausePopUpTransform, 0.2f, () =>
            {
                pausePopUpTransform.gameObject.SetActive(false);
                popUps.SetActive(false);
            });
        }
    }
}