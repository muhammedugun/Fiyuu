// Refactor 09.05.24
using UnityEngine;


/// <summary>
/// Oyuncunun giriþ kontröllerinden sorumlu
/// </summary>
public class ControllerManager : MonoBehaviour
{

    public static PlayerControllerAction controller;

    private void Awake()
    {
        controller = new PlayerControllerAction(); 
    }

    private void OnDisable()
    {
        controller.Disable();
    }
    private void OnEnable()
    {
        controller.Enable();
    }

}
