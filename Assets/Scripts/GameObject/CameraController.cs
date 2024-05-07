using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    [SerializeField] private MMF_Player beginningFeedback;
    [SerializeField] private MMF_Player skipBeginningFeedback;

    private void OnEnable()
    {
        ControllerManager.Subscribe(SkipBeginning);
    }


    public void SkipBeginning(InputAction.CallbackContext context)
    {
        ControllerManager.Unsubscribe(SkipBeginning);
        if (beginningFeedback.IsPlaying)
        {
            beginningFeedback.StopFeedbacks();
            skipBeginningFeedback.PlayFeedbacks();
        }
    }

}
