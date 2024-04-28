using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    [SerializeField] private MMF_Player beginningFeedback;
    [SerializeField] private MMF_Player skipBeginningFeedback;
    // Start is called before the first frame update
    void Start()
    {
        ControllerManager.action.InLevel.Attack.started += SkipBeginning;
    }

    public void SkipBeginning(InputAction.CallbackContext context)
    {
        ControllerManager.action.InLevel.Attack.started -= SkipBeginning;
        beginningFeedback.StopFeedbacks();
        skipBeginningFeedback.PlayFeedbacks();

    }

}
