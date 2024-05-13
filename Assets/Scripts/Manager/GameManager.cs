using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Oyun ak���n� y�netmekten sorumlu
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
