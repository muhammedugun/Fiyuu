// Refactor 11.03.24
using UnityEngine;

/// <summary>
/// Oyuncunun giriþ kontröllerinden sorumlu
/// </summary>
public class ControllerManager : MonoBehaviour
{
    [SerializeField] private AmmoSelectionUI ammoSelectionUI;

    internal static PlayerControllerAction action;
    

    private void Awake()
    {
        action = new PlayerControllerAction();
    }

    private void OnDisable()
    {
        action.Disable();
    }
    private void OnEnable()
    {
        action.Enable();
    }

    private void Update()
    {
        AmmoSelectForUI();
    }

    /// <summary>
    /// UI için mühimmat seçimini yapar. Hangi mühimmat seçildiyse UI'da ona karþýlýk gelen ikonu highlight yapar.
    /// </summary>
    private void AmmoSelectForUI()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ammoSelectionUI.SetAmmoType(AmmoMatter.Wood);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ammoSelectionUI.SetAmmoType(AmmoMatter.Stone);
    }
}
