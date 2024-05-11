using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevelPopUp : MonoBehaviour
{
    public void OnClickNextLevel()
    {
        GameManager.ResumeLevel();
        string thisSceneName = SceneManager.GetActiveScene().name;
        int levelNumber = int.Parse(thisSceneName.Substring(5, thisSceneName.Length - 5));
        Infrastructure.LoadScene("Level" + (levelNumber + 1));
    }

    public void OnClickMainMenu()
    {
        GameManager.ResumeLevel();
        Infrastructure.LoadScene("MainMenu");
    }

    public void OnClickRestartButton()
    {
        GameManager.RestartLevel();
    }
}
