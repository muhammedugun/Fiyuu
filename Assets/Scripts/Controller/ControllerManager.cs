// Refactor 11.03.24
using UnityEngine;

/// <summary>
/// Oyuncunun giriþ kontröllerinden sorumlu
/// </summary>
public class ControllerManager : MonoBehaviour
{

    internal static PlayerControllerAction action;
    

    private void Awake()
    {
        action = new PlayerControllerAction();
    }

    private void OnDisable()
    {
        action.Disable();
    }
    private void OnEnable()
    {
        action.Enable();
    }



}
