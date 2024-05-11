//Refactor 11.05.24

using UnityEngine;

public class SettingsPopUp : MonoBehaviour
{
    [SerializeField] private GameObject _soundButtonOn, _soundButtonOff, _fullScreenButtonOn, _fullScreenButtonOff;

    private const string muteKey = "isMute";
    private const string fullScreenKey = "isNotFullScreen";

    private void Start()
    {
        InitializeSoundToggle();
        InitializeFullScreenToggle();
    }

    private void InitializeSoundToggle()
    {
        bool isMuted = PlayerPrefs.GetInt(muteKey) == 1;
        SetSoundState(!isMuted);
    }

    private void InitializeFullScreenToggle()
    {
        bool isFullScreen = PlayerPrefs.GetInt(fullScreenKey) != 1;
        SetFullScreenState(isFullScreen);
    }

    private void SetSoundState(bool isSoundOn)
    {
        AudioListener.volume = isSoundOn ? 1f : 0f;
        Toggle(_soundButtonOn, _soundButtonOff, isSoundOn);
        PlayerPrefs.SetInt(muteKey, isSoundOn ? 0 : 1);
    }

    private void SetFullScreenState(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        Toggle(_fullScreenButtonOn, _fullScreenButtonOff, isFullScreen);
        PlayerPrefs.SetInt(fullScreenKey, isFullScreen ? 0 : 1);
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
}
