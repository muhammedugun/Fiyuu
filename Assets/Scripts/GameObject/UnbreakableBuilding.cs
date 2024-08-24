// Refactor 23.08.24
using MoreMountains.Feedbacks;
using UnityEngine;

/// <summary>
/// Kýrýlamaz binalarla ilgili fonksiyonlardan sorumlu
/// </summary>
public class UnbreakableBuilding : MonoBehaviour
{
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
        skipBeginningFeedback.PlayFeedbacks();
        EventBus.Unsubscribe(EventType.FirstClickInLevel, SkipBeginning);
    }
}
