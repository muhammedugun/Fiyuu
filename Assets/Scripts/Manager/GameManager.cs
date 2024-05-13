using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Oyun akýþýný yönetmekten sorumlu
/// </summary>
public class GameManager : MonoBehaviour
{
    private static ThrowInputController _throwInputController;

    private void Start()
    {
        _throwInputController = FindObjectOfType<ThrowInputController>();
    }

    public static void PauseLevel()
    {
        _throwInputController.isEnabled = false;
        Time.timeScale = 0f;
    }

    public static void ResumeLevel()
    {
        _throwInputController.isEnabled = true;
        Time.timeScale = 1f;
    }

    public static void RestartLevel()
    {
        ResumeLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
