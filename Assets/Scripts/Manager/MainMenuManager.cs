using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Image _soundButtonImage;
    [SerializeField] private Sprite _soundOnSprite, _soundOffSprite;

    private const string _muteKey = "isMute";

    private void Start()
    {
        InitializeSoundToggle();
    }

    public void LoadScene(string sceneName)
    {
        Infrastructure.LoadScene(sceneName);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void OpenURL()
    {
        Application.OpenURL("https://spritestudio.itch.io/fiyuu");
    }

    /// <summary>
    /// Popup a��l��� i�in ses toggle'�n� ayarlar
    /// </summary>
    private void InitializeSoundToggle()
    {
        bool isMuted = PlayerPrefs.GetInt(_muteKey) == 1;
        //bool isMuted = YandexGame.savesData.isMute == 1;
        SetSoundState(!isMuted);
    }

    /// <summary>
    /// Sesin a��kl�k kapal�l�k durumunu ayarlar
    /// </summary>
    private void SetSoundState(bool isSoundOn)
    {
        AudioListener.volume = isSoundOn ? 1f : 0f;
        UpdateSoundButtonSprite();
        PlayerPrefs.SetInt(_muteKey, isSoundOn ? 0 : 1);
        PlayerPrefs.Save();
        //YandexGame.savesData.isMute = isSoundOn ? 0 : 1;
        //YandexGame.SaveProgress();

    }

    /// <summary>
    /// Ses butonunun sprite'�n� g�nceller
    /// </summary>
    private void UpdateSoundButtonSprite()
    {
        bool isSoundOn = AudioListener.volume != 0f;
        _soundButtonImage.sprite = isSoundOn ? _soundOnSprite : _soundOffSprite;
    }

    public void SoundToggle()
    {
        bool isSoundOn = AudioListener.volume != 0f;
        SetSoundState(!isSoundOn);
    }
}
