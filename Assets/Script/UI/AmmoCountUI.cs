// Refactor: 11.02.24
using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Arayüzde, kalan mühimmat sayýsýný gösteren obje ile ilgili.
/// </summary>
public class AmmoCountUI : MonoBehaviour
{
    /// <summary>
    /// Mühimmat tükendi bildirimi
    /// </summary>
    public static event Action OnOutOfAmmo;
    /// <summary>
    /// Mühimmat sýnýrý
    /// </summary>
    public int limitCount;

    /// <summary>
    /// Ateþlenmiþ mühimmat sayýsý
    /// </summary>
    internal int firedCount;

    /// <summary>
    /// Kalan mühimmat sayýsýnýn yazýldýðý text objesi
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
    /// Ateþlenmiþ mühimmat sayýsýný arttýrýr
    /// </summary>
    private void IncreaseFiredAmmo()
    {
        firedCount++;
    }

    /// <summary>
    /// Mühimmatýn tükenip tükenmediðini kontrol et
    /// </summary>
    private void CheckOutOfAmmo()
    {
        if (limitCount - firedCount <= 0)
        {
            OnOutOfAmmo.Invoke();
        }
    }

    /// <summary>
    /// Mühimmat sayýsýnýn yazýldýðý text objesini güncelle
    /// </summary>
    private void UpdateText()
    {
        text.text = (limitCount - firedCount).ToString();
    }


}
