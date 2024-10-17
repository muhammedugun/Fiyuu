// Refactor 23.08.24
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

/// <summary>
/// Chapters men�s�n� y�netmekten sorumlu
/// </summary>
public class ChaptersManager : MonoBehaviour
{
    [SerializeField] private GameObject _playPopUp;
    [SerializeField] private GameObject _chapter1;
    [SerializeField] private GameObject _lockImagePrefab;
    [SerializeField] private List<Image> _stars;
    [SerializeField] Text _popUpTitleText;
    [SerializeField] Text _scoreText;
    [SerializeField] private Sprite _lockIcon;
    [SerializeField] private RectTransform _playPopUpWindow;

    private string _levelName;

    private void Start()
    {
        CanEnterLevel();
    }

    /// <summary>
    /// Harita �zerinde bir seviyeye bas�ld���nda a��lmas� gereken men�y� a�ar
    /// </summary>
    /// <param name="levelNumber"></param>
    public void OpenPlayPopUp(int levelNumber)
    {
        if (IsLevelCompleted(levelNumber))
        {
            _playPopUp.SetActive(true);
            _popUpTitleText.text = "Level " + levelNumber;
            _levelName = "Level" + levelNumber;
            //_scoreText.text = PlayerPrefs.GetInt("LevelScore" + levelNumber).ToString();
            _scoreText.text = YandexGame.savesData.LevelScore[levelNumber].ToString();
            // if (PlayerPrefs.GetInt("LevelStars" + levelNumber) >= 3)
            if (YandexGame.savesData.LevelStars[levelNumber] >= 3)
            {
                foreach (var star in _stars)
                {
                    star.color = new Color(star.color.r, star.color.g, star.color.b, 1f);
                    var child = star.transform.GetChild(0).GetComponent<Image>();
                    child.color = new Color(child.color.r, child.color.g, child.color.b, 1f);
                }
            }
            //else if (PlayerPrefs.GetInt("LevelStars" + levelNumber) >= 2)
            else if (YandexGame.savesData.LevelStars[levelNumber] >= 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    _stars[i].color = new Color(_stars[i].color.r, _stars[i].color.g, _stars[i].color.b, 1f);
                    var child = _stars[i].transform.GetChild(0).GetComponent<Image>();
                    child.color = new Color(child.color.r, child.color.g, child.color.b, 1f);
                }

            }
            //else if (PlayerPrefs.GetInt("LevelStars" + levelNumber) >= 1)
            else if (YandexGame.savesData.LevelStars[levelNumber] >= 1)
            {
                _stars[2].color = new Color(_stars[2].color.r, _stars[2].color.g, _stars[2].color.b, 1f);
                var child = _stars[2].transform.GetChild(0).GetComponent<Image>();
                child.color = new Color(child.color.r, child.color.g, child.color.b, 1f);
            }
        }
        else if (levelNumber == 1)
        {
            Debug.LogWarning("Level1open");
            _playPopUp.SetActive(true);
            _popUpTitleText.text = "Level " + levelNumber;
            _levelName = "Level" + levelNumber;
        }

    }

    /// <summary>
    /// Play popup� kapat�r
    /// </summary>
    public void ClosePlayPopUp()
    {
        _scoreText.text = "0";
        foreach (var star in _stars)
        {
            star.color = new Color(star.color.r, star.color.g, star.color.b, 0.5f);
            var child = star.transform.GetChild(0).GetComponent<Image>();
            child.color = new Color(child.color.r, child.color.g, child.color.b, 0.5f);
        }
        _levelName = null;

        _playPopUpWindow.localScale = Vector3.one;

        _playPopUpWindow.DOScale(Vector3.zero, 0.2f)
            .SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                _playPopUp.SetActive(false);
            });
    }

    /// <summary>
    /// Oynanmak istenen seviyeyi y�kler
    /// </summary>
    public void LoadLevel()
    {
        if (_levelName != null)
            Infrastructure.LoadScene(_levelName);
    }

    // Bir b�l�m� tamamlad���nda �a�r�lacak fonksiyon
    public static void CompleteLevel(int levelIndex)
    {
        // B�l�m� tamamland���n� kaydedin
        //PlayerPrefs.SetInt("CompletedLevel" + levelIndex, 1);
        YandexGame.savesData.CompletedLevel[levelIndex] = 1;
        // De�i�iklikleri kaydedin
        //PlayerPrefs.Save();
        YandexGame.SaveProgress();
    }


    // Belirli bir b�l�m� tamamlay�p tamamlamad���n� kontrol eden fonksiyon
    public bool IsLevelCompleted(int levelIndex)
    {
        // return PlayerPrefs.GetInt("CompletedLevel" + levelIndex, 0) == 1;
        return YandexGame.savesData.CompletedLevel[levelIndex] == 1;
    }

    // Belirli bir b�l�me girebilir mi?
    public void CanEnterLevel()
    {
        // �nceki t�m b�l�mlerin tamamlan�p tamamlanmad���n� kontrol eder
        for (int i = 2; i <= 10; i++)
        {
            if (!IsLevelCompleted(i))
            {
                if (i >= 10)
                {
                    _chapter1.transform.GetChild(i - 1).GetChild(0).gameObject.SetActive(false);
                    _chapter1.transform.GetChild(i - 1).GetChild(1).gameObject.SetActive(false);
                    Instantiate(_lockImagePrefab, _chapter1.transform.GetChild(i - 1));
                }
                else
                    _chapter1.transform.GetChild(i - 1).GetChild(0).GetComponent<Image>().sprite = _lockIcon;
            }
        }

    }

    public void LoadScene(string sceneName)
    {
        Infrastructure.LoadScene(sceneName);
    }
}