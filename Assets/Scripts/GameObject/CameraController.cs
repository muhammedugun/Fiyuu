// Refactor 23.08.24
using MoreMountains.Feedbacks;
using UnityEngine;

/// <summary>
/// Kamera davranýþlarýndan sorumlu
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private MMF_Player beginningFeedback;
    [SerializeField] private MMF_Player skipBeginningFeedback;

    private void OnEnable()
    {
        EventBus.Subscribe(EventType.FirstClickInLevel, SkipBeginning);

    }

    /// <summary>
    /// Baþlangýç feedback'lerini atlar
    /// </summary>
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
