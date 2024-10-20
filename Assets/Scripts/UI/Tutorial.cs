using MoreMountains.Feedbacks;
using UnityEngine;

/// <summary>
/// Oyun i�indeki e�itimleri y�netmekten sorumludur
/// </summary>
public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _tutorial;
    [SerializeField] private MMF_Player _playFeedbacks, _stopFeedbacks;
    [SerializeField] private bool _isTutorialScene;
    [SerializeField] private bool _isFirstTutorial;
    [SerializeField] private Transform _launcherArm;
    [SerializeField] private GameObject _pauseButton;

    private ThrowInputController _throwInputController;
    private bool _isInputControl;
    private bool _isDeactive;

    private void Start()
    {
        _throwInputController = FindObjectOfType<ThrowInputController>();
    }

    private void OnDisable()
    {
        ThrowInputController.Started -= Deactive;
    }

    private void Update()
    {
        if (_isFirstTutorial && _launcherArm.eulerAngles.z > 80 && _launcherArm.eulerAngles.z < 89)
        {
            _isFirstTutorial = false;
            _pauseButton.SetActive(false);
            GameManager.PauseLevel();
            _throwInputController.isEnabled = true;
            Active();
        }
        else if (_isFirstTutorial && _launcherArm.eulerAngles.z < 320 && !_isInputControl)
        {
            _isInputControl = true;
            _throwInputController.isEnabled = false;
        }
    }

    /// <summary>
    /// E�itimi aktifle�tirir
    /// </summary>
    public void Active()
    {
        if (_isTutorialScene)
        {
            ThrowInputController.Started += Deactive;
            for (int i = 0; i < 2; i++)
            {
                _tutorial.transform.GetChild(i).gameObject.SetActive(true);
            }
            _playFeedbacks.PlayFeedbacks();
        }
    }

    /// <summary>
    /// E�itimi devre d��� b�rak�r
    /// </summary>
    private void Deactive()
    {
        _isDeactive = true;
        ThrowInputController.Started -= Deactive;
        GameManager.ResumeLevel();
        if (_pauseButton != null)
            _pauseButton.SetActive(true);
        for (int i = 0; i < 2; i++)
        {
            _tutorial.transform.GetChild(i).gameObject.SetActive(false);
        }
        _stopFeedbacks.PlayFeedbacks();
    }

    public void PlayFeedbacks()
    {
        if (!_isDeactive)
            _playFeedbacks.PlayFeedbacks();
    }
}
