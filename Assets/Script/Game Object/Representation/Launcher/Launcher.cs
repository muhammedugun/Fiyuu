using System;
using UnityEngine;

/// <summary>
/// F�rlat�c� silahlar�(manc�n�klar�) temsil eder.
/// </summary>
public abstract class Launcher : MonoBehaviour
{
    public event Action OnLaunched;
    [SerializeField] protected Transform _ammoSpawnPosition;
    [SerializeField] protected AmmoMatter ammoType;
    [SerializeField] protected GameObject[] ammoTypePrefabs;
    [Tooltip("M�himmat�n rigidbody bile�eni")]
    protected Rigidbody _ammoRigidBody;
    [Tooltip("F�rlatma g�c�")]
    [SerializeField] protected float _launchPower;
    [SerializeField] protected Animator _animator;
    protected int _ammoTypeIndex;
    protected GameObject _ammo;
    [SerializeField] protected Transform _ammoParent;
    /// <summary>
    /// M�himmat�n ba�lang��taki y pozisyonu
    /// </summary>
    protected float _ammoStartY;
    protected float _currentLaunchPower;
    private void Awake()
    {
        _currentLaunchPower = _launchPower;
    }

    protected virtual void SetLaunchPower()// ba�lang��ta a��rl��� atamak i�in
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
    /// F�rlatma modundan ��kt���nda yap�lacaklar
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
                Debug.LogWarning("M�himmat tipi i�in uygun m�himmat prefab� atanmam��");
            }
        }
    }

    /// <summary>
    /// Ba�lang��ta yap�lacaklar
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
    /// F�rlatma i�lemini ba�lat�r
    /// </summary>
    /// <param name="context"></param>
    protected abstract void Launch();

    /// <summary>
    /// Animasyon state de�i�imi i�in kontrol ger�ekle�tir. 
    /// </summary>
    /// <returns>State de�i�ebilecek durumdaysa true, de�ilse false d�nd�r�r</returns>
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
