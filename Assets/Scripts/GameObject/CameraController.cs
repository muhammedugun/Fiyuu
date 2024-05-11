using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{

    [SerializeField] private MMF_Player beginningFeedback;
    [SerializeField] private MMF_Player skipBeginningFeedback;

    private void OnEnable()
    {
        EventBus.Subscribe(EventType.FirstClickInLevel, SkipBeginning);

    }

    public void SkipBeginning()
    {
        if (beginningFeedback.IsPlaying)
        {
            beginningFeedback.StopFeedbacks();
            skipBeginningFeedback.PlayFeedbacks();
            EventBus.Unsubscribe(EventType.FirstClickInLevel, SkipBeginning);
        }
    }

}
