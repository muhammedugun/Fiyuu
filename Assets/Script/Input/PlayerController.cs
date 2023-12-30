using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Oyuncunun giriþ kontröllerinden sorumlu
/// </summary>
public class PlayerController : MonoBehaviour
{
    internal static PlayerControllerAction action;
    internal static float attackStartedTime;
    internal static float attackEndedTime;
    internal static bool isPerformed;
    private void Awake()
    {
        action = new PlayerControllerAction();
       
        //action.InLevel.Attack.started += SetAttackStartedTime;
        //action.InLevel.Attack.canceled += SetAttackEndedTime;
    }

    private void OnDisable()
    {
        action.Disable();
    }
    private void OnEnable()
    {
        action.Enable();
    }

    private void SetAttackStartedTime(InputAction.CallbackContext context)
    {
        isPerformed = true;
        attackStartedTime = Time.time;
    }

    private void SetAttackEndedTime(InputAction.CallbackContext context)
    {
        isPerformed=false;
        attackEndedTime = Time.time;
    }


}
