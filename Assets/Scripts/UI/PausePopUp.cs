//Refactor 11.05.24

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePopUp : MonoBehaviour
{
    [SerializeField] private Image _soundButtonImage;
    [SerializeField] private Sprite _soundOnSprite, _soundOffSprite;
    [SerializeField] private RectTransform _pausePopUpTransform;
    [SerializeField] private GameObject _popUps;
    [SerializeField] private Text title;
    
    private const string volumeKey = "isMute";

    private void Start()
    {
        int levelIndex = int.Parse(SceneManager.GetActiveScene().name.Substring(5));
        title.text = "Level " + levelIndex;
    }

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
        if(!_pausePopUpTransform.gameObject.activeSelf)
        {
            GameManager.PauseLevel();
            OpenPausePopUp();
        }
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