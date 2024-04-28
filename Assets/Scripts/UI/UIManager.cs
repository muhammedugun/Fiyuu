using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject InLevelPopUps;
    [SerializeField] private GameObject areYouSure;

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClickPauseButton()
    {
        InLevelPopUps.SetActive(true);
        InLevelPopUps.transform.Find("Pause").gameObject.SetActive(true);
        PauseGame();
    }

    public void OnClickResumeButton()
    {
        ResumeGame();
        InLevelPopUps.transform.Find("Pause").gameObject.SetActive(false);
        InLevelPopUps.SetActive(false);
    }

    public void OnClickRestartButton()
    {
        RestartLevel();
    }

    public void OpenAreYouSure()
    {
        areYouSure.SetActive(true);
    }

    public void CloseAreYouSure()
    {
        areYouSure.SetActive(false);
    }

    public void OpenMainMenu()
    {
        Infrastructure.LoadScene("MainMenu");
    }

    public void OpenNextLevel()
    {
        string thisSceneName = SceneManager.GetActiveScene().name;
        int levelNumber = int.Parse(thisSceneName.Substring(5, thisSceneName.Length - 5));
        Infrastructure.LoadScene("Level"+(levelNumber+1));

    }

}
