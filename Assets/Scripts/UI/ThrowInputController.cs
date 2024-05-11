// Refactor 09.05.24
using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Fýrlatma inputu ile ilgili görevlerden sorumlu
/// </summary>
public class ThrowInputController : MonoBehaviour, IPointerDownHandler
{
    public static event Action Started;

    public void OnPointerDown(PointerEventData eventData)
    {
        Started?.Invoke();
    }

}
