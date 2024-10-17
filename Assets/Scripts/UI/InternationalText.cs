using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class InternationalText : MonoBehaviour
{
    [SerializeField] private bool _isTextMeshPro = true;
    [SerializeField] private string _tr, _en, _ru;

    void Start()
    {
        if (_isTextMeshPro)
            UpdateTMP();
        else
            UpdateText();
    }

    private void UpdateTMP()
    {
        if (YandexGame.lang == "tr")
        {
            GetComponent<TextMeshProUGUI>().text = _tr;
        }
        else if (YandexGame.lang == "ru")
        {
            GetComponent<TextMeshProUGUI>().text = _ru;
        }
        else
        {
            GetComponent<TextMeshProUGUI>().text = _en;
        }
    }

    private void UpdateText()
    {
        if (YandexGame.lang == "tr")
        {
            GetComponent<Text>().text = _tr;
        }
        else if (YandexGame.lang == "ru")
        {
            GetComponent<Text>().text = _ru;
        }
        else
        {
            GetComponent<Text>().text = _en;
        }
    }



    public string GetText()
    {
        if (YandexGame.lang == "tr")
        {
            return _tr;
        }
        else if (YandexGame.lang == "ru")
        {
            return _ru;
        }
        else
        {
            return _en;
        }
    }

}
