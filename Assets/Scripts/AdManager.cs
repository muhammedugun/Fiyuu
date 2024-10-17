using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class AdManager : MonoBehaviour
{
    /// <summary>
    /// Belirtilen saniye cinsinden zaman sonra reklam gösterilecek.
    /// </summary>
    private float _adTimeInterval = 90f;
    private static float _lastAdShowTime = 0f;
    public void FullscreenShow()
    {
        if (Time.time - _lastAdShowTime > _adTimeInterval)
        {
            _lastAdShowTime = Time.time;
            YandexGame.FullscreenShow();
        }
    }
}
