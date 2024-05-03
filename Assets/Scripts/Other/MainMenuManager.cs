
using DG.Tweening;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPopUp;
    [SerializeField] private RectTransform settingsPopUpWindow;

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
        settingsPopUpWindow.localScale = Vector3.one;

        settingsPopUpWindow.DOScale(Vector3.zero, 0.2f)
                 .SetEase(Ease.InOutQuad).OnComplete(() =>
                 {
                     settingsPopUp.SetActive(false);
                 });
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    
}
