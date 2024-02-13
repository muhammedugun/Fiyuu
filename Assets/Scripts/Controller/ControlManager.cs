using UnityEngine;

public class ControlManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainVirtualCamera;
    [SerializeField] private Catapult _catapult;
    [SerializeField] private CatapultMovement _catapultMovement;
    [SerializeField] private FreeRoamCamera _freeRoamCamera;
    [SerializeField] private AmmoSelectionUI _ammoSelection;

    private void Update()
    {
        if(_mainVirtualCamera.activeSelf && Input.GetMouseButtonDown(1))
        {
            _mainVirtualCamera.SetActive(false);
            _catapult.enabled = false;
            _catapultMovement.enabled = false;
            _freeRoamCamera.enabled = true;
        }
        else if (!_mainVirtualCamera.activeSelf && (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)))
        {
            _mainVirtualCamera.SetActive(true);
            _catapult.enabled = true;
            _catapultMovement.enabled = true;
            _freeRoamCamera.enabled = false;
        }
        AmmoSelection();
    }

    private void AmmoSelection()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            _ammoSelection.SetAmmoType(AmmoMatter.Wood);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            _ammoSelection.SetAmmoType(AmmoMatter.Stone);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            _ammoSelection.SetAmmoType(AmmoMatter.Iron);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            _ammoSelection.SetAmmoType(AmmoMatter.Steel);
    }
}
