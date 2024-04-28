//Refactor: 11.02.24
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Arayüzdeki mühimmat seçim objesiyle ilgili
/// </summary>
public class AmmoManager : MonoBehaviour
{

    public List<AmmoMatter> ammunition;

    /// <summary>
    /// Seviyedeki mühimmat sayýsý
    /// </summary>
    [HideInInspector] public int levelAmmoCount;

    [SerializeField] private GameObject ammoGridItems;
    [SerializeField] private GameObject plusIcon;

    [SerializeField] private Sprite woodImage;
    [SerializeField] private Sprite stoneImage;


    private void Start()
    {
        levelAmmoCount = ammunition.Count;

        UpdateAmmunition();
        UpdateAmmoIcons();

    }

    private void OnEnable()
    {
        EventBus.Subscribe(EventType.LauncherThrowed, RemoveAtAmmunition);

    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.LauncherThrowed, RemoveAtAmmunition);
    }

    private void UpdateAmmunition()
    {
        switch (ammunition.Count)
        {
            case 1:
                for (int i = 0; i < 2; i++)
                    ammoGridItems.transform.GetChild(i).gameObject.SetActive(false);
                plusIcon.SetActive(false);
                break;

            case 2:
                ammoGridItems.transform.GetChild(0).gameObject.SetActive(false);
                plusIcon.SetActive(false);
                break;
            case 3:
                plusIcon.SetActive(false);
                break;
            default:
                plusIcon.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+" + (ammunition.Count - 3).ToString();
                break;
        }
    }

    private void UpdateAmmoIcons()
    {
        int cycle = Math.Min(3, ammunition.Count);

        for (int i = 0; i < cycle; i++)
        {
            if (ammunition[i] == AmmoMatter.Wood)
            {
                ammoGridItems.transform.GetChild(2 - i).GetChild(0).GetChild(1).GetComponent<Image>().sprite = woodImage;
                ammoGridItems.transform.GetChild(2 - i).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "Wood";
            }
            if (ammunition[i] == AmmoMatter.Stone)
            {
                ammoGridItems.transform.GetChild(2 - i).GetChild(0).GetChild(1).GetComponent<Image>().sprite = stoneImage;
                ammoGridItems.transform.GetChild(2 - i).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "Stone";
            }
        }
    }

    private void RemoveAtAmmunition()
    {
        ammunition.RemoveAt(0);
        UpdateAmmunition();
        UpdateAmmoIcons();
        if (ammunition.Count <= 0)
            EventBus.Publish(EventType.OutOfAmmo);
    }

}
