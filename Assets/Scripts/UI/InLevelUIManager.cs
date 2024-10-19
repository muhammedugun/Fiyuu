using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InLevelUIManager : MonoBehaviour
{
    [SerializeField] private Text _levelTitle;

    void Start()
    {
        //int levelIndex = int.Parse(SceneManager.GetActiveScene().name.Substring(5));
        //_levelTitle.text = "Level " + levelIndex;

        _levelTitle.text = _levelTitle.GetComponent<InternationalText>().GetText();
    }

}
