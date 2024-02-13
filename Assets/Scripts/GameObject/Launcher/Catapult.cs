using UnityEngine;
using Zenject;


public class Catapult : Launcher
{
    public AmmoMatter ammoType;
    public GameObject[] ammoTypePrefabs;

    private void Start()
    {
        Initialize();
        Invoke("SetLaunchPower", 0.1f); // birkaç frame sonra çalýþtýrmak amaçlý
    }

    private void Update()
    {
        var inputValue = PlayerController.action.InLevel.Move.ReadValue<Vector3>();
        if (inputValue.magnitude>0)
        {
            Move(inputValue);
        }
    }

    void Move(Vector3 inputValue)
    {
        transform.Rotate(0f,-inputValue.z,0f);
    }

    internal bool initialized;
    /// <summary>
    /// Baþlangýçta yapýlacaklar
    /// </summary>
    internal virtual void Initialize()
    {
        initialized = false;
        if (_ammo != null && _ammo.transform.position == _ammoSpawnPosition.position)
        {
            Destroy(_ammo);
        }
        _ammo = Instantiate(ammoTypePrefabs[(int)ammoType - 1], _ammoParent);
        _ammo.transform.position = _ammoSpawnPosition.position;
        _ammo.transform.rotation = _ammoSpawnPosition.rotation;
        _ammoRigidBody = _ammo.GetComponent<Rigidbody>();
    }


    /// <summary>
    /// Fýrlatma iþlemini baþlatýr
    /// </summary>
    public void Launch(float speed)
    {
        _ammoRigidBody.isKinematic = false;
        _ammoRigidBody.useGravity = true;
        _ammoRigidBody.AddForce(_ammoRigidBody.transform.up * _currentLaunchPower * speed /* *(1+(firingBar.currentFillAmount*100f))*/);
        _ammoRigidBody.transform.parent = null;
        OnLaunchedInvoke();
        Invoke(nameof(Initialize),1f);
    }

    /// <summary>
    /// Fýrlatma modundan çýktýðýnda yapýlacaklar
    /// </summary>
    public void LaunchExit()
    {
        Initialize();
        _ammoRigidBody.isKinematic = true;
        _ammoRigidBody.useGravity = false;
    }


    /// <summary>
    /// Mühimmat tipini ayarlar
    /// </summary>
    /// <param name="ammoMatter"></param>
    public void SetAmmoType(AmmoMatter ammoMatter)
    {
        if (gameObject != null)
        {
            ammoType = ammoMatter;
            Initialize();
            UpdateLaunchPowerInvoke();
        }
        else
        {
            Debug.LogWarning("Catapult objesi null");
        }
    }

}
