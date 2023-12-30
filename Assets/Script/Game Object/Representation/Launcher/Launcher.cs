using System;
using UnityEngine;

/// <summary>
/// Fýrlatýcý silahlarý(mancýnýklarý) temsil eder.
/// </summary>
public abstract class Launcher : MonoBehaviour
{
    public event Action OnLaunched;
    [SerializeField] protected Transform _ammoSpawnPosition;
    [SerializeField] protected AmmoMatter ammoType;
    [SerializeField] protected GameObject[] ammoTypePrefabs;
    [Tooltip("Mühimmatýn rigidbody bileþeni")]
    protected Rigidbody _ammoRigidBody;
    [Tooltip("Fýrlatma gücü")]
    [SerializeField] protected float _launchPower;
    [SerializeField] protected Animator _animator;
    protected int _ammoTypeIndex;
    protected GameObject _ammo;
    [SerializeField] protected Transform _ammoParent;
    /// <summary>
    /// Mühimmatýn baþlangýçtaki y pozisyonu
    /// </summary>
    protected float _ammoStartY;
    protected float _currentLaunchPower;
    private void Awake()
    {
        _currentLaunchPower = _launchPower;
    }

    protected virtual void SetLaunchPower()// baþlangýçta aðýrlýðý atamak için
    {
        _currentLaunchPower = _launchPower * _ammoRigidBody.mass;
    }

    public virtual void SetAmmoType(int ammoTypeIndex)
    {
        ammoType = (AmmoMatter)ammoTypeIndex;
        AssignAmmoType();
        Initialize();
        Invoke("SetLaunchPower", 0.1f);
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
    /// Ammo tipini ata
    /// </summary>
    protected virtual void AssignAmmoType()
    {
        Debug.Log("ammotype:" + ammoType);
        for (int i = 0; i < ammoTypePrefabs.Length; i++)
        {
            if (ammoTypePrefabs[i].GetComponent<Ammo>().matter == ammoType)
            {
                _ammoTypeIndex = i;
                break;
            }
            else if (i == ammoTypePrefabs.Length - 1)
            {
                Debug.LogWarning("Mühimmat tipi için uygun mühimmat prefabý atanmamýþ");
            }
        }
    }

    /// <summary>
    /// Baþlangýçta yapýlacaklar
    /// </summary>
    protected virtual void Initialize()
    {
        if (_ammo != null && _ammo.transform.position == _ammoSpawnPosition.position)
        {
            Destroy(_ammo);
        }
        _ammo = Instantiate(ammoTypePrefabs[_ammoTypeIndex], _ammoParent);
        _ammo.transform.position = _ammoSpawnPosition.position;
        _ammo.transform.rotation = _ammoSpawnPosition.rotation;
        _ammoRigidBody = _ammo.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Fýrlatma iþlemini baþlatýr
    /// </summary>
    /// <param name="context"></param>
    protected abstract void Launch();

    /// <summary>
    /// Animasyon state deðiþimi için kontrol gerçekleþtir. 
    /// </summary>
    /// <returns>State deðiþebilecek durumdaysa true, deðilse false döndürür</returns>
    protected virtual bool CheckChangeAnimState()
    {

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Empty"))
            return true;
        else return false;
    }

    protected void OnLaunchedInvoke()
    {
        OnLaunched?.Invoke();
    }
}
