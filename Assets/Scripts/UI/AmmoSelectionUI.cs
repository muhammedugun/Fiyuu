// Refactor 23.08.24
using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

/// <summary>
/// Level i�indeki m�himmat se�im ekran�n� y�netmekten sorumludur
/// </summary>
public class AmmoSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject _ammoGrid;
    [SerializeField] private GameObject _plusIcon;
    [SerializeField] private Sprite _woodImage;
    [SerializeField] private Sprite _stoneImage;

    private AmmoManager _ammoManager;

    private const int _MaxDisplayedAmmo = 3;

    private void Start()
    {
        _ammoManager = FindObjectOfType<AmmoManager>();
        UpdateUI();
    }

    private void OnEnable()
    {
        EventBus.Subscribe(EventType.AmmoReduced, UpdateUI);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.AmmoReduced, UpdateUI);
    }

    /// <summary>
    /// M�himmat se�im ekran�n� g�nceller
    /// </summary>
    private void UpdateUI()
    {
        UpdateAmmunitionDisplay();
        UpdateAmmoIcons();
        UpdatePlusIcon();
    }

    /// <summary>
    /// Kalan m�himmat say�s�n� g�nceller
    /// </summary>
    private void UpdateAmmunitionDisplay()
    {
        int ammoCount = _ammoManager.ammunition.Count;

        for (int i = 0; i < _MaxDisplayedAmmo; i++)
        {
            bool isActive = i < ammoCount;
            _ammoGrid.transform.GetChild(i).gameObject.SetActive(isActive);
        }

        _plusIcon.SetActive(ammoCount > _MaxDisplayedAmmo);
    }

    /// <summary>
    /// M�himmatlar�n ikonlar�n� g�nceller
    /// </summary>
    private void UpdateAmmoIcons()
    {
        int ammoCount = Math.Min(_MaxDisplayedAmmo, _ammoManager.ammunition.Count);

        for (int i = 0; i < ammoCount; i++)
        {
            Transform ammoSlot = _ammoGrid.transform.GetChild(_MaxDisplayedAmmo - i - 1);
            Transform ammoIcon = ammoSlot.GetChild(0);

            switch (_ammoManager.ammunition[i])
            {
                case AmmoMatter.Wood:
                    SetAmmoIcon(ammoIcon, _woodImage, "Wood");
                    break;
                case AmmoMatter.Stone:
                    SetAmmoIcon(ammoIcon, _stoneImage, "Stone");
                    break;
            }
        }
    }

    /// <summary>
    /// Parametre olarak g�nderilen m�himmat�n ikonunu g�nceller
    /// </summary>
    /// <param name="ammoIcon"></param>
    /// <param name="image"></param>
    /// <param name="text"></param>
    private void SetAmmoIcon(Transform ammoIcon, Sprite image, string text)
    {
        Image iconImage = ammoIcon.GetChild(1).GetComponent<Image>();
        Text iconText = ammoIcon.GetChild(0).GetChild(0).GetComponent<Text>();

        iconImage.sprite = image;
        if (YandexGame.savesData.language == "tr")
        {
            if (text == "Wood") iconText.text = "Odun";
            else if (text == "Stone") iconText.text = "Taş";
        }
        else
            iconText.text = text;

    }

    /// <summary>
    /// Plus ikonunun �zerindeki say�y� g�nceller
    /// </summary>
    private void UpdatePlusIcon()
    {
        if (_plusIcon.activeSelf)
        {
            int ammoCount = _ammoManager.ammunition.Count;
            _plusIcon.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+" + (ammoCount - _MaxDisplayedAmmo).ToString();
        }
    }
}
