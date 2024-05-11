using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UnbreakableBuilding : MonoBehaviour
{
    [SerializeField] private MMF_Player skipBeginningFeedback;

    private void OnEnable()
    {
        EventBus.Subscribe(EventType.FirstClickInLevel, SkipBeginning);
    }

    public void SkipBeginning()
    {
        skipBeginningFeedback.PlayFeedbacks();
        EventBus.Unsubscribe(EventType.FirstClickInLevel, SkipBeginning);
    }
}
