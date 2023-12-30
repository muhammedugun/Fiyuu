using UnityEngine;
using Zenject;

public class AmmoSelection : MonoBehaviour
{
    //[Inject] Trebuchet trebuchet;
    [Inject] Catapult catapult;
    // Start is called before the first frame update
    public void SetAmmoType(int ammoTypeIndex)
    {
        //if (trebuchet != null) trebuchet.SetAmmoType(ammoTypeIndex);
        if (catapult != null) catapult.SetAmmoType(ammoTypeIndex);
    }
}
