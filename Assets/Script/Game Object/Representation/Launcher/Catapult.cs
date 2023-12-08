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
        Invoke("SetLaunchPower", 0.1f); // birka� frame sonra �al��t�rmak ama�l�
    }
    /// <summary>
    /// Ba�lang��ta yap�lacaklar
    /// </summary>
    private void Initialize()
    {
        _ammo = Instantiate(_ammoPrefab, _ammoParent);
        _ammo.transform.position = _ammoSpawnPosition.position;
        _ammoRigidBody = _ammo.GetComponent<Rigidbody>();
    }

    void SetLaunchPower()// ba�lang��ta a��rl��� atamak i�in
    {
        _launchPower *= _ammoRigidBody.mass;
    }

    /// <summary>
    /// F�rlat�rken yap�lmas� gerekirken
    /// </summary>
    protected override void Launch()
    {
        _ammoRigidBody.isKinematic = false;
        _ammoRigidBody.useGravity = true;
        _ammoRigidBody.AddForce(_ammoRigidBody.transform.up * _launchPower * (1+(firingBar.currentFillAmount*100f)));
        _ammoRigidBody.transform.parent = null;
    }

    /// <summary>
    /// F�rlatma modundan ��kt���nda yap�lacaklar
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
