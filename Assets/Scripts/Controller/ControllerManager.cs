// Refactor 11.03.24
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Oyuncunun giriþ kontröllerinden sorumlu
/// </summary>
public class ControllerManager : MonoBehaviour
{

    private static PlayerControllerAction playerAction;
    private static List<Action<InputAction.CallbackContext>> methodList = new List<Action<InputAction.CallbackContext>>();

    private void Awake()
    {
        playerAction = new PlayerControllerAction(); 
    }

    private void OnDisable()
    {
        Remove();
        playerAction.Disable();
    }
    private void OnEnable()
    {
        playerAction.Enable();
    }

    public static void Subscribe(Action<InputAction.CallbackContext> method)
    {
        methodList.Add(method);
        playerAction.InLevel.Attack.started += method;

    }

    public static void Unsubscribe(Action<InputAction.CallbackContext> method)
    {
        methodList.Remove(method);
        playerAction.InLevel.Attack.started -= method;
    }

    private void Remove()
    {
        foreach (var method in methodList)
        {
            playerAction.InLevel.Attack.started -= method;
        }
        
    }

    public static void UpdateLog()
    {

        foreach (var method in methodList)
        {
            Debug.Log($"Method Name: {method.Method.Name}, Declaring Type: {method.Method.DeclaringType}");
        }
    }

}
