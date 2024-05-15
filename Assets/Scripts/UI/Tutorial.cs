using MoreMountains.Feedbacks;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _tutorial;
    [SerializeField] private MMF_Player playFeedbacks, stopFeedbacks;
    [SerializeField] private bool _isTutorialScene;
    [SerializeField] private bool _isFirstTutorial;
    [SerializeField] private Transform _launcherArm;
    [SerializeField] private GameObject _pauseButton;

    private ThrowInputController _throwInputController;
    private bool isInputControl;
    private bool isDeactive;

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
        if (_isFirstTutorial && _launcherArm.eulerAngles.z>70 && _launcherArm.eulerAngles.z < 80)
        {
            _isFirstTutorial = false;
            _pauseButton.SetActive(false);
            GameManager.PauseLevel();
            _throwInputController.isEnabled = true;
            Active();
        }
        else if(_isFirstTutorial && _launcherArm.eulerAngles.z<320 && !isInputControl)
        {
            isInputControl = true;
            _throwInputController.isEnabled = false;
        }
    }
    public void Active()
    {
        if(_isTutorialScene)
        {
            ThrowInputController.Started += Deactive;
            for (int i = 0; i < 2; i++)
            {
                _tutorial.transform.GetChild(i).gameObject.SetActive(true);
            }
            playFeedbacks.PlayFeedbacks();
        }
    }

    private void Deactive()
    {
        isDeactive = true;
        ThrowInputController.Started -= Deactive;
        GameManager.ResumeLevel();
        if(_pauseButton!=null)
            _pauseButton.SetActive(true);
        for (int i = 0; i < 2; i++)
        {
            _tutorial.transform.GetChild(i).gameObject.SetActive(false);
        }
        stopFeedbacks.PlayFeedbacks();
    }

    public void PlayFeedbacks()
    {
        if(!isDeactive)
            playFeedbacks.PlayFeedbacks();
    }

}
