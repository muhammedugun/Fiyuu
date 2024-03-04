using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputRangeTest : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPointerDown = false;
    public event Action performed;
    public event Action started;
    public event Action exit;

    void Update()
    {
        if (isPointerDown)
        {
            OnPerformed();
        }
    }

    void OnPerformed()
    {
        performed?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        started?.Invoke();
        isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        exit?.Invoke();
    }
}
