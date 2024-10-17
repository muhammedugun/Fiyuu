//Refactor 23.08.24

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

/// <summary>
/// Level i�indeki pause men��s�n� y�netmekten sorumludur
/// </summary>
public class PausePopUp : MonoBehaviour
{
    [SerializeField] private Image _soundButtonImage;
    [SerializeField] private Sprite _soundOnSprite, _soundOffSprite;
    [SerializeField] private RectTransform _pausePopUpTransform;
    [SerializeField] private GameObject _popUps;
    [SerializeField] private Text _title;

    private const string _volumeKey = "isMute";

    private void Start()
    {
        int levelIndex = int.Parse(SceneManager.GetActiveScene().name.Substring(5));
        _title.text = "Level " + levelIndex;
    }

    private void OnEnable()
    {
        UpdateSoundButtonSprite();
    }

    /// <summary>
    /// Ses butonunun sprite'�n� g�nceller
    /// </summary>
    private void UpdateSoundButtonSprite()
    {
        bool isSoundOn = AudioListener.volume != 0f;
        _soundButtonImage.sprite = isSoundOn ? _soundOnSprite : _soundOffSprite;
    }

    /// <summary>
    /// Ses butonuna bas�l�nca yap�lmas� gereken i�lemleri yapar. 
    /// Sesi azalt�r ya da art�r�r, sprite'� g�nceller, ses bilgisini g�nceller
    /// </summary>
    public void OnClickSoundButton()
    {
        bool isMuted = AudioListener.volume == 0f;
        AudioListener.volume = isMuted ? 1f : 0f;
        //PlayerPrefs.SetInt(_volumeKey, isMuted ? 0 : 1);
        //PlayerPrefs.Save();
        YandexGame.savesData.isMute = isMuted ? 0 : 1;
        YandexGame.SaveProgress();
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
        if (!_pausePopUpTransform.gameObject.activeSelf)
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
            UIAnimationService.ClosePopUp(_pausePopUpTransform, 0.2f, true, () =>
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
            UIAnimationService.OpenPopUp(_pausePopUpTransform, 0.2f, true);
        }
    }

}