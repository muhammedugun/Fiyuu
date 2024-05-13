using System;
using UnityEngine;
using UnityEngine.UI;

public class AmmoSelectionUI : MonoBehaviour
{
    
    [SerializeField] private GameObject ammoGrid;
    [SerializeField] private GameObject plusIcon;
    [SerializeField] private Sprite woodImage;
    [SerializeField] private Sprite stoneImage;

    private AmmoManager _ammoManager;
    private const int MaxDisplayedAmmo = 3;

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

    private void UpdateUI()
    {
        UpdateAmmunitionDisplay();
        UpdateAmmoIcons();
    }

    private void UpdateAmmunitionDisplay()
    {
        int ammoCount = _ammoManager.ammunition.Count;

        for (int i = 0; i < MaxDisplayedAmmo; i++)
        {
            bool isActive = i < ammoCount;
            ammoGrid.transform.GetChild(i).gameObject.SetActive(isActive);
        }

        plusIcon.SetActive(ammoCount > MaxDisplayedAmmo);
    }

    private void UpdateAmmoIcons()
    {
        int ammoCount = Math.Min(MaxDisplayedAmmo, _ammoManager.ammunition.Count);

        for (int i = 0; i < ammoCount; i++)
        {
            Transform ammoSlot = ammoGrid.transform.GetChild(MaxDisplayedAmmo - i - 1);
            Transform ammoIcon = ammoSlot.GetChild(0);

            switch (_ammoManager.ammunition[i])
            {
                case AmmoMatter.Wood:
                    SetAmmoIcon(ammoIcon, woodImage, "Wood");
                    break;
                case AmmoMatter.Stone:
                    SetAmmoIcon(ammoIcon, stoneImage, "Stone");
                    break;
            }
        }
    }

    private void SetAmmoIcon(Transform ammoIcon, Sprite image, string text)
    {
        Image iconImage = ammoIcon.GetChild(1).GetComponent<Image>();
        Text iconText = ammoIcon.GetChild(0).GetChild(0).GetComponent<Text>();

        iconImage.sprite = image;
        iconText.text = text;
    }
}
