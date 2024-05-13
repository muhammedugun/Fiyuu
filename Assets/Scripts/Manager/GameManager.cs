using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Oyun akýþýný yönetmekten sorumlu
/// </summary>
public class GameManager : MonoBehaviour
{
    public static void PauseLevel()
    {
        Time.timeScale = 0f;
    }

    public static void ResumeLevel()
    {
        Time.timeScale = 1f;
    }

    public static void RestartLevel()
    {
        ResumeLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
