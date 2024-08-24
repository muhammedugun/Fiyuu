using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Infrastructure.LoadScene(sceneName);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void OpenURL()
    {
        Application.OpenURL("https://spritestudio.itch.io/fiyuu");
    }
}
