// Refactor 09.05.24
using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// F�rlatma inputu ile ilgili g�revlerden sorumlu
/// </summary>
public class ThrowInputController : MonoBehaviour, IPointerDownHandler
{
    public static event Action Started;
    public bool isEnabled;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(isEnabled)
            Started?.Invoke();
    }

    public void SetIsEnabled(bool condition)
    {
        isEnabled = condition;
    }
}
