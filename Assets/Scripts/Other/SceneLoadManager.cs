using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Sahneler yŁklemelerinden sorumlu
/// </summary>
public class SceneLoadManager : MonoBehaviour
{
    private void Awake()
    {
        ControllerManager.action.InLevel.RestartScene.started += RestartCurrentScene;
    }

    public void RestartCurrentScene(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}