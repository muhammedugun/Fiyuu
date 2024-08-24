// Refactor 24.08.24
using UnityEngine;

public class SettingsPopUp : MonoBehaviour
{
    [SerializeField] private GameObject _soundButtonOn, _soundButtonOff, _fullScreenButtonOn, _fullScreenButtonOff;
    [SerializeField] private RectTransform _backgroundFade, _window;

    private const string _muteKey = "isMute";
    private const string _fullScreenKey = "isNotFullScreen";

    private void Start()
    {
        InitializeSoundToggle();
        InitializeFullScreenToggle();
    }

    /// <summary>
    /// Popup açýlýþý için ses toggle'ýný ayarlar
    /// </summary>
    private void InitializeSoundToggle()
    {
        bool isMuted = PlayerPrefs.GetInt(_muteKey) == 1;
        SetSoundState(!isMuted);
    }

    /// <summary>
    /// Popup açýlýþý için tam ekran toggle'ýný ayarlar
    /// </summary>
    private void InitializeFullScreenToggle()
    {
        bool isFullScreen = PlayerPrefs.GetInt(_fullScreenKey) != 1;
        SetFullScreenState(isFullScreen);
    }

    /// <summary>
    /// Sesin açýklýk kapalýlýk durumunu ayarlar
    /// </summary>
    private void SetSoundState(bool isSoundOn)
    {
        AudioListener.volume = isSoundOn ? 1f : 0f;
        Toggle(_soundButtonOn, _soundButtonOff, isSoundOn);
        PlayerPrefs.SetInt(_muteKey, isSoundOn ? 0 : 1);
    }

    /// <summary>
    /// Tam ekran açýklýk kapalýlýk durumunu ayarlar
    /// </summary>
    private void SetFullScreenState(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        Toggle(_fullScreenButtonOn, _fullScreenButtonOff, isFullScreen);
        PlayerPrefs.SetInt(_fullScreenKey, isFullScreen ? 0 : 1);
    }

    private void Toggle(GameObject onObject, GameObject offObject, bool isOn)
    {
        onObject.SetActive(isOn);
        offObject.SetActive(!isOn);
    }

    public void SoundToggle()
    {
        bool isSoundOn = AudioListener.volume != 0f;
        SetSoundState(!isSoundOn);
    }

    public void FullScreenToggle()
    {
        SetFullScreenState(!Screen.fullScreen);
    }

    public void OpenPopUp()
    {
        _backgroundFade.gameObject.SetActive(true);
        _window.gameObject.SetActive(true);

        UIAnimationService.OpenPopUp(_window, 0.2f, true);
    }

    public void ClosePopUp()
    {
        UIAnimationService.ClosePopUp(_window, 0.2f, true, () =>
        {
            _backgroundFade.gameObject.SetActive(false);
            _window.gameObject.SetActive(false);
        });
    }
}
