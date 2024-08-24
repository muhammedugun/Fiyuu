// Refactor 23.08.24
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Level içindeki mühimmat seçim ekranýný yönetmekten sorumludur
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
    /// Mühimmat seçim ekranýný günceller
    /// </summary>
    private void UpdateUI()
    {
        UpdateAmmunitionDisplay();
        UpdateAmmoIcons();
        UpdatePlusIcon();
    }

    /// <summary>
    /// Kalan mühimmat sayýsýný günceller
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
    /// Mühimmatlarýn ikonlarýný günceller
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
    /// Parametre olarak gönderilen mühimmatýn ikonunu günceller
    /// </summary>
    /// <param name="ammoIcon"></param>
    /// <param name="image"></param>
    /// <param name="text"></param>
    private void SetAmmoIcon(Transform ammoIcon, Sprite image, string text)
    {
        Image iconImage = ammoIcon.GetChild(1).GetComponent<Image>();
        Text iconText = ammoIcon.GetChild(0).GetChild(0).GetComponent<Text>();

        iconImage.sprite = image;
        iconText.text = text;
    }

    /// <summary>
    /// Plus ikonunun üzerindeki sayýyý günceller
    /// </summary>
    private void UpdatePlusIcon()
    {
        if(_plusIcon.activeSelf)
        {
            int ammoCount = _ammoManager.ammunition.Count;
            _plusIcon.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+" + (ammoCount - _MaxDisplayedAmmo).ToString();
        }
    }
}
