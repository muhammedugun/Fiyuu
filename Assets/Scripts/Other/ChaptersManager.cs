using UnityEngine;
using UnityEngine.UI;

public class ChaptersManager : MonoBehaviour
{
    [SerializeField] private GameObject playPopUp;
    [SerializeField] Text popUpTitleText;
    string levelName;

    public void OpenPlayPopUp(int levelNumber)
    {
        playPopUp.SetActive(true);
        popUpTitleText.text = "Level " + levelNumber;
        levelName = "Level" + levelNumber;
    }

    public void ClosePlayPopUp()
    {
        levelName = null;
        playPopUp.SetActive(false);
        
    }

    public void LoadLevel()
    {
        if(levelName!=null)
            Infrastructure.LoadScene(levelName);
    }

}
