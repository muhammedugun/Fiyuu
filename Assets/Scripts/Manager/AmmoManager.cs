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


    private void Start()
    {
        levelAmmoCount = ammunition.Count;
    }

    private void OnEnable()
    {
        EventBus.Subscribe(EventType.LauncherThrowed, ReduceAmmunition);

    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.LauncherThrowed, ReduceAmmunition);
    }

    /// <summary>
    /// Cephaneyi azalt
    /// </summary>
    private void ReduceAmmunition()
    {
        if(ammunition.Count>0)
        {
            ammunition.RemoveAt(0);
            EventBus.Publish(EventType.AmmoReduced);
        }
        if (ammunition.Count <= 0)
            EventBus.Publish(EventType.OutOfAmmo);
    }

}
