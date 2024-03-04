//Refactor: 11.02.24
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Aray�zdeki m�himmat se�im objesiyle ilgili
/// </summary>
public class AmmoSelectionUI : MonoBehaviour
{

    [SerializeField] private List<Image> _ammoSelections;

    //[Inject] private Catapult _catapult; 
    public void SetAmmoType(AmmoMatter ammoMatter)
    {
        
        //_catapult.SetAmmoType(ammoMatter);
        SetAmmoSelectionColor(ammoMatter);
    }
    
    /// <summary>
    /// M�himmatlar�n se�im rengini ayarlar
    /// </summary>
    /// <param name="ammoMatter"></param>
    private void SetAmmoSelectionColor(AmmoMatter ammoMatter)
    {
        ResetAmmoSelectionColor();
        _ammoSelections[(int)ammoMatter-1].color = Color.yellow;
    }
    private void ResetAmmoSelectionColor()
    {
        foreach(var ammo in _ammoSelections)
        {
            ammo.color = Color.white;
        }
    }
}
