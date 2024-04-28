using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPopUp;

    public void LoadScene(string sceneName)
    {
        Infrastructure.LoadScene(sceneName);
    }

    public void OpenSettingsPopUp()
    {
        settingsPopUp.SetActive(true);
    }

    public void CloseSettingsPopUp()
    {
        settingsPopUp.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
