using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaptersManager : MonoBehaviour
{
    [SerializeField] private GameObject playPopUp;
    [SerializeField] private GameObject chapter1;
    [SerializeField] private GameObject lockImagePrefab;
    [SerializeField] private List<Image> stars;
    [SerializeField] Text popUpTitleText;
    [SerializeField] Text scoreText;
    [SerializeField] private Sprite lockIcon;
    [SerializeField] private RectTransform playPopUpWindow;

    string levelName;

    private void Start()
    {
        CanEnterLevel();
    }

    public void OpenPlayPopUp(int levelNumber)
    {
        if(IsLevelCompleted(levelNumber))
        {
            playPopUp.SetActive(true);
            popUpTitleText.text = "Level " + levelNumber;
            levelName = "Level" + levelNumber;
            scoreText.text = PlayerPrefs.GetInt("LevelScore" + levelNumber).ToString();
            if (PlayerPrefs.GetInt("LevelStars" + levelNumber) >= 3)
            {
                foreach (var star in stars)
                {
                    star.color = new Color(star.color.r, star.color.g, star.color.b, 1f);
                    var child = star.transform.GetChild(0).GetComponent<Image>();
                    child.color = new Color(child.color.r, child.color.g, child.color.b, 1f);
                }
            }
            else if (PlayerPrefs.GetInt("LevelStars" + levelNumber) >= 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, 1f);
                    var child = stars[i].transform.GetChild(0).GetComponent<Image>();
                    child.color = new Color(child.color.r, child.color.g, child.color.b, 1f);
                }

            }
            else if (PlayerPrefs.GetInt("LevelStars" + levelNumber) >= 1)
            {
                stars[2].color = new Color(stars[2].color.r, stars[2].color.g, stars[2].color.b, 1f);
                var child = stars[2].transform.GetChild(0).GetComponent<Image>();
                child.color = new Color(child.color.r, child.color.g, child.color.b, 1f);
            }
        }
        else if (levelNumber == 1)
        {
            Debug.LogWarning("Level1open");
            playPopUp.SetActive(true);
            popUpTitleText.text = "Level " + levelNumber;
            levelName = "Level" + levelNumber;
        }

    }

    public void ClosePlayPopUp()
    {
        scoreText.text = "0";
        foreach (var star in stars)
        {
            star.color = new Color(star.color.r, star.color.g, star.color.b, 0.5f);
            var child = star.transform.GetChild(0).GetComponent<Image>();
            child.color = new Color(child.color.r, child.color.g, child.color.b, 0.5f);
        }
        levelName = null;

        playPopUpWindow.localScale = Vector3.one;

        playPopUpWindow.DOScale(Vector3.zero, 0.2f)
                 .SetEase(Ease.InOutQuad).OnComplete(() =>
                 {
                     playPopUp.SetActive(false);
                 });
        
    }

    public void LoadLevel()
    {
        if(levelName!=null)
            Infrastructure.LoadScene(levelName);
    }

    // Bir bölümü tamamladýðýnda çaðrýlacak fonksiyon
    public static void CompleteLevel(int levelIndex)
    {
        // Bölümü tamamlandýðýný kaydedin
        PlayerPrefs.SetInt("CompletedLevel" + levelIndex, 1);

        // Deðiþiklikleri kaydedin
        PlayerPrefs.Save();
    }


    // Belirli bir bölümü tamamlayýp tamamlamadýðýný kontrol eden fonksiyon
    public bool IsLevelCompleted(int levelIndex)
    {
        return PlayerPrefs.GetInt("CompletedLevel" + levelIndex, 0) == 1;
    }



    // Belirli bir bölüme girebilir mi?
    public void CanEnterLevel()
    {
        // Önceki tüm bölümlerin tamamlanýp tamamlanmadýðýný kontrol eder
        for (int i = 2; i <= 10; i++)
        {
            if (!IsLevelCompleted(i))
            {
                if(i>=10)
                {
                    chapter1.transform.GetChild(i - 1).GetChild(0).gameObject.SetActive(false);
                    chapter1.transform.GetChild(i - 1).GetChild(1).gameObject.SetActive(false);
                    Instantiate(lockImagePrefab, chapter1.transform.GetChild(i - 1));
                }
                else
                    chapter1.transform.GetChild(i - 1).GetChild(0).GetComponent<Image>().sprite = lockIcon;
            }
        }
        
    }

    public void LoadScene(string sceneName)
    {
        Infrastructure.LoadScene(sceneName);
    }



}