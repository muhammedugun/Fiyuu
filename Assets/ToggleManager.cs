using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleManager : MonoBehaviour
{
    public GameObject soundButtonOn, soundButtonOff, fullScreenButtonOn, fullScreenButtonOff;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Volume")==0)
        {
            AudioListener.volume = 0f;
            soundButtonOn.SetActive(false);
            soundButtonOff.SetActive(true);
        }
        else
        {
            AudioListener.volume = 1f;
            soundButtonOn.SetActive(true);
            soundButtonOff.SetActive(false);
        }

        if (PlayerPrefs.GetInt("isNotFullScreen") == 1)
        {
            Screen.fullScreen = false;
            fullScreenButtonOn.SetActive(false);
            fullScreenButtonOff.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("isNotFullScreen", 0);
            Screen.fullScreen = true;
            fullScreenButtonOn.SetActive(true);
            fullScreenButtonOff.SetActive(false);
        }

    }


    public void SoundToggle()
    {
        if (AudioListener.volume == 0f)
        {
            AudioListener.volume = 1f;
            PlayerPrefs.SetInt("Volume", 1);
            soundButtonOn.SetActive(true);
            soundButtonOff.SetActive(false);
        }
        else
        {
            AudioListener.volume = 0f;
            PlayerPrefs.SetInt("Volume", 0);
            soundButtonOn.SetActive(false);
            soundButtonOff.SetActive(true);
        }
    }

    public void FullScreenToggle()
    {
        if (Screen.fullScreen)
        {
            PlayerPrefs.SetInt("isNotFullScreen", 1);
            fullScreenButtonOn.SetActive(false);
            fullScreenButtonOff.SetActive(true);
        }  
        else
        {
            PlayerPrefs.SetInt("isNotFullScreen", 0);
            fullScreenButtonOn.SetActive(true);
            fullScreenButtonOff.SetActive(false);
        }
        Screen.fullScreen = !Screen.fullScreen;


    }
}
