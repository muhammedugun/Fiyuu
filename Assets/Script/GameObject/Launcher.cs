
using System;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    /// <summary>
    /// Fırlatma gerçekleşti eventi
    /// </summary>
    public static event Action OnLaunched;

    [Tooltip("Mühimmatın rigidbody bileşeni")]
    [SerializeField] private Rigidbody ammoRigidBody;
    [SerializeField] private GameObject[] ammoTypePrefabs;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _ammoSpawnPosition;
    [SerializeField] private Transform _ammoParent;
    [SerializeField] private AmmoMatter ammoType;
    [Tooltip("Fırlatma gücü")]
    [SerializeField] private float _launchPower;

    private GameObject _ammo;
    private GameObject lastAmmo;
    private InputRange inputRange;
    /// <summary>
    /// Mühimmatın başlangıçtaki y pozisyonu
    /// </summary>
    private float _ammoStartY;
    private float _currentLaunchPower;
    private int _ammoTypeIndex;


    private void Awake()
    {
        _currentLaunchPower = _launchPower;
    }

    private void Start()
    {
        AssignAmmoType();
        Initialize();
        _ammoStartY = ammoRigidBody.transform.position.y;
        Invoke("SetLaunchPower", 0.1f);

    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    private void Subscribe()
    {
        inputRange = FindObjectOfType<InputRange>();
        inputRange.started += Launch;
    }

    private void UnSubscribe()
    {
        inputRange.started -= Launch;
    }

    /// <summary>
    /// başlangıçta ağırlığı atamak için
    /// </summary>
    private void SetLaunchPower()
    {
        _currentLaunchPower = _launchPower * ammoRigidBody.mass;
    }

    public void SetAmmoType(int ammoTypeIndex)
    {
        ammoType = (AmmoMatter)ammoTypeIndex;
        AssignAmmoType();
        Initialize();
        Invoke("SetLaunchPower", 0.1f);
    }

    /// <summary>
    /// Fırlatma modundan çıktığında yapılacaklar
    /// </summary>
    public void LaunchExit()
    {
        Initialize();
        ammoRigidBody.isKinematic = true;
        ammoRigidBody.useGravity = false;
    }

    /// <summary>
    /// Ammo tipini ata
    /// </summary>
    private void AssignAmmoType()
    {
        for (int i = 0; i < ammoTypePrefabs.Length; i++)
        {
            if (ammoTypePrefabs[i].GetComponent<Ammo>().matter == ammoType)
            {
                _ammoTypeIndex = i;
                break;
            }
            else if (i == ammoTypePrefabs.Length - 1)
            {
                Debug.LogWarning("Mühimmat tipi için uygun mühimmat prefabı atanmamış");
            }
        }
    }

    /// <summary>
    /// Başlangıçta yapılacaklar
    /// </summary>
    private void Initialize()
    {
        if (_ammo != null && _ammo.transform.position == _ammoSpawnPosition.position)
        {
            Destroy(_ammo);
        }
        _ammo = Instantiate(ammoTypePrefabs[_ammoTypeIndex], _ammoParent);
        if (lastAmmo == null)
            lastAmmo = _ammo;
        _ammo.transform.position = _ammoSpawnPosition.position;
        _ammo.transform.rotation = _ammoSpawnPosition.rotation;
        ammoRigidBody = _ammo.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Animasyon state değişimi için kontrol gerçekleştir. 
    /// </summary>
    /// <returns>State değişebilecek durumdaysa true, değilse false döndürür</returns>
    private bool CheckChangeAnimState()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Empty"))
            return true;
        else return false;
    }

    private void OnLaunchedInvoke()
    {
        OnLaunched?.Invoke();
    }

    /// <summary>
    /// Fırlatma animasyonunu tetikle
    /// </summary>
    private void LaunchAnimTrigger()
    {
        _animator.SetTrigger("launch");
    }

    /// <summary>
    /// Fırlatma işlemini başlatır
    /// </summary>
    private void Launch()
    {
        if (CheckChangeAnimState()) { LaunchAnimTrigger(); }
        else if (ammoRigidBody.isKinematic)
        {
            _ammo.GetComponent<Ammo>().GetComponent<TrailRenderer>().enabled = true;
            _ammo.GetComponent<Ammo>().launchPos = _ammo.transform.position;
            if (lastAmmo!=null && lastAmmo != _ammo)
            {
                lastAmmo.GetComponent<Ammo>().isDestroyable=true;
                if(lastAmmo.GetComponent<ExplosiveBase>().isExplode)
                Destroy(lastAmmo.gameObject);
            }
                
            lastAmmo = _ammo;

            _animator.SetTrigger("leave");
            ammoRigidBody.isKinematic = false;
            ammoRigidBody.useGravity = true;
            ammoRigidBody.GetComponent<Collider>().enabled = true;
            float height = ammoRigidBody.transform.position.y - _ammoStartY;
            Debug.Log(height);
            if(height<2)
            {
                height++;
            }
            
            ammoRigidBody.AddForce(ammoRigidBody.transform.up * _currentLaunchPower * height);
            ammoRigidBody.transform.parent = null;
        }
        OnLaunchedInvoke();
        Invoke(nameof(Initialize), 0.1f);
    }
}
