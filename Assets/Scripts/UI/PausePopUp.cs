//Refactor 11.05.24

using UnityEngine;
using UnityEngine.UI;

public class PausePopUp : MonoBehaviour
{
    [SerializeField] private Image _soundButtonImage;
    [SerializeField] private Sprite _soundOnSprite, _soundOffSprite;
    [SerializeField] private RectTransform _pausePopUpTransform;
    [SerializeField] private GameObject _popUps;

    
    private const string volumeKey = "isMute";


    private void OnEnable()
    {
        UpdateSoundButtonSprite();
    }

    private void UpdateSoundButtonSprite()
    {
        bool isSoundOn = AudioListener.volume != 0f;
        _soundButtonImage.sprite = isSoundOn ? _soundOnSprite : _soundOffSprite;
    }

    public void OnClickSoundButton()
    {
        bool isMuted = AudioListener.volume == 0f;
        AudioListener.volume = isMuted ? 1f : 0f;
        PlayerPrefs.SetInt(volumeKey, isMuted ? 0 : 1);
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
        OpenPausePopUp();
    }

    public void OnClickMainMenu()
    {
        GameManager.ResumeLevel();
        Infrastructure.LoadScene("MainMenu");
    }


    private void ClosePausePopUp()
    {
        if (_pausePopUpTransform != null)
        {
            UIAnimation.ClosePopUp(_pausePopUpTransform, 0.2f, true, () =>
            {
                _pausePopUpTransform.gameObject.SetActive(false);
            });
        }
    }


    private void OpenPausePopUp()
    {
        if (_pausePopUpTransform != null)
        {
            _popUps.SetActive(true);
            _pausePopUpTransform.gameObject.SetActive(true);
            UIAnimation.OpenPopUp(_pausePopUpTransform, 0.2f, true);
        }
    }

}