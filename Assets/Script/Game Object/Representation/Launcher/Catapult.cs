using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;


public class Catapult : Launcher
{
    [Inject] FiringBar firingBar;
    [SerializeField] GameObject _ammoPrefab;
    [SerializeField] Transform _ammoSpawnPosition;
    [SerializeField] Transform _ammoParent;
    GameObject _ammo;

    private void Start()
    {
        Initialize();
        PlayerController.action.InLevel.Attack.canceled += LaunchAnimTrigger;
        Invoke("SetLaunchPower", 0.1f); // birkaç frame sonra çalýþtýrmak amaçlý
    }
    /// <summary>
    /// Baþlangýçta yapýlacaklar
    /// </summary>
    private void Initialize()
    {
        _ammo = Instantiate(_ammoPrefab, _ammoParent);
        _ammo.transform.position = _ammoSpawnPosition.position;
        _ammoRigidBody = _ammo.GetComponent<Rigidbody>();
    }

    void SetLaunchPower()// baþlangýçta aðýrlýðý atamak için
    {
        _launchPower *= _ammoRigidBody.mass;
    }

    /// <summary>
    /// Fýrlatýrken yapýlmasý gerekirken
    /// </summary>
    protected override void Launch()
    {
        _ammoRigidBody.isKinematic = false;
        _ammoRigidBody.useGravity = true;
        _ammoRigidBody.AddForce(_ammoRigidBody.transform.up * _launchPower * (1+(firingBar.currentFillAmount*100f)));
        _ammoRigidBody.transform.parent = null;
    }

    /// <summary>
    /// Fýrlatma modundan çýktýðýnda yapýlacaklar
    /// </summary>
    protected void LaunchExit()
    {
        Initialize();
        _ammoRigidBody.isKinematic = true;
        _ammoRigidBody.useGravity = false;
        
    }


    protected void LaunchAnimTrigger(InputAction.CallbackContext context)
    {
        if(CheckChangeAnimState())
        {
            _animator.SetTrigger("launch");
            _animator.speed = (firingBar.currentFillAmount * 6f) + 1;
        }
        
    }

}
