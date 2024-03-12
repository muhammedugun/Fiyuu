// Refactor: 11.02.24
using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Aray�zde, kalan m�himmat say�s�n� g�steren obje ile ilgili.
/// </summary>
public class AmmoCountUI : MonoBehaviour
{
    /// <summary>
    /// M�himmat t�kendi bildirimi
    /// </summary>
    public static event Action OnOutOfAmmo;
    /// <summary>
    /// M�himmat s�n�r�
    /// </summary>
    public int limitCount;

    /// <summary>
    /// Ate�lenmi� m�himmat say�s�
    /// </summary>
    internal int firedCount;

    /// <summary>
    /// Kalan m�himmat say�s�n�n yaz�ld��� text objesi
    /// </summary>
    [SerializeField] private TextMeshProUGUI text;


    private void Start()
    {
        text.text = limitCount.ToString();
        Subscribe();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    private void Subscribe()
    {
        Launcher.OnLaunched += IncreaseFiredAmmo;
        Launcher.OnLaunched += UpdateText;
        Launcher.OnLaunched += CheckOutOfAmmo;
    }

    private void UnSubscribe()
    {
        Launcher.OnLaunched -= IncreaseFiredAmmo;
        Launcher.OnLaunched -= UpdateText;
        Launcher.OnLaunched -= CheckOutOfAmmo;
    }

    /// <summary>
    /// Ate�lenmi� m�himmat say�s�n� artt�r�r
    /// </summary>
    private void IncreaseFiredAmmo()
    {
        firedCount++;
    }

    /// <summary>
    /// M�himmat�n t�kenip t�kenmedi�ini kontrol et
    /// </summary>
    private void CheckOutOfAmmo()
    {
        if (limitCount - firedCount <= 0)
        {
            OnOutOfAmmo.Invoke();
        }
    }

    /// <summary>
    /// M�himmat say�s�n�n yaz�ld��� text objesini g�ncelle
    /// </summary>
    private void UpdateText()
    {
        text.text = (limitCount - firedCount).ToString();
    }


}
