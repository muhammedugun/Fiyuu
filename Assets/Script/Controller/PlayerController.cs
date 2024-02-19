using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Oyuncunun giri� kontr�llerinden sorumlu
/// </summary>
public class PlayerController : MonoBehaviour
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
