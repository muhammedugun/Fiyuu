using Blobcreate.ProjectileToolkit;
using System;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    /// <summary>
    /// Fırlatma gerçekleşti eventi
    /// </summary>
    public static event Action OnLaunched;
    [Tooltip("Projectile için son transform noktası")]
    public Transform endTransform;

    [Header("Ammo")]
    [SerializeField] private GameObject[] ammoTypePrefabs;
    [SerializeField] private Transform ammoSpawnPosition;
    [SerializeField] private AmmoMatter ammoType;

    [Header("Project Tile")]
    [SerializeField] TrajectoryPredictor trajectoryPredictor;

    [Header("Other")]
    [SerializeField] private Animator animator;
    [Tooltip("Fırlatma gücü")]
    [SerializeField] private float launchPower;

    private Vector3 _launchVelocity;
    private GameObject _ammo;
    private GameObject _lastAmmo;
    private InputRange _inputRange;
    private Rigidbody _ammoRigidBody;
    /// <summary>
    /// Mühimmatın başlangıçtaki y pozisyonu
    /// </summary>
    private float _ammoStartY;
    private int _ammoTypeIndex;
    private float _height;

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    private void Start()
    {
        AssignAmmoType();
        _ammoStartY = ammoSpawnPosition.position.y;
        CreateAmmo();
    }

    private void LateUpdate()
    {
        if(_ammo!=null)
        {
          _height = _ammo.transform.position.y - _ammoStartY;
          if(_height>.02f)
          {
                if(animator.GetCurrentAnimatorStateInfo(0).IsName("Firing") || animator.GetCurrentAnimatorStateInfo(0).IsName("Mirror Firing"))
                    trajectoryPredictor.enabled = true;
                _launchVelocity = _ammo.transform.up * launchPower * _height;
                trajectoryPredictor.Render(_ammo.transform.position, _launchVelocity, endTransform.position, 25);
          }
        }
    }

    private void Subscribe()
    {
        _inputRange = FindObjectOfType<InputRange>();
        _inputRange.started += Launch;
    }

    private void UnSubscribe()
    {
        _inputRange.started -= Launch;
    }

    /// <summary>
    /// Mühimmat tipini ata
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
    /// Mühimmat tipini değiştir
    /// </summary>
    /// <param name="ammoTypeIndex"></param>
    public void SetAmmoType(int ammoTypeIndex)
    {
        ammoType = (AmmoMatter)ammoTypeIndex;
        AssignAmmoType();
        CreateAmmo();
    }

    /// <summary>
    /// Mühimmat oluşturur
    /// </summary>
    private void CreateAmmo()
    {
        // mevcut bir mühimmat varsa ve fırlatılmamışsa onu yok et
        if (CheckAnimStateEmpty() && _ammo != null && _ammo.transform.position == ammoSpawnPosition.transform.position)
            Destroy(_ammo.gameObject);

        _ammo = Instantiate(ammoTypePrefabs[_ammoTypeIndex], ammoSpawnPosition);
        if (_lastAmmo == null)
            _lastAmmo = _ammo;
        _ammo.transform.position = ammoSpawnPosition.position;
        _ammo.transform.rotation = ammoSpawnPosition.rotation;
        _ammoRigidBody = _ammo.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Empty animasyonunda mıyım diye kontrol eder
    /// </summary>
    /// <returns>Empty animasyonundaysa true, değilse false döndürür</returns>
    private bool CheckAnimStateEmpty()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Empty"))
            return true;
        else return false;
    }

    /// <summary>
    /// Fırlatma animasyonunu tetikle
    /// </summary>
    private void LaunchAnimTrigger()
    {
        animator.SetBool("leave", false);
        animator.SetTrigger("launch");
    }

    /// <summary>
    /// Fırlatma işlemini başlatır
    /// </summary>
    private void Launch()
    {
        if (CheckAnimStateEmpty())
        {
            LaunchAnimTrigger();
        }
        else if (_ammoRigidBody.isKinematic && !animator.GetBool("leave"))
        {
            trajectoryPredictor.enabled = false;
            _ammo.GetComponent<Ammo>().GetComponent<TrailRenderer>().enabled = true;
            _ammo.GetComponent<Ammo>().launchPos = _ammo.transform.position;
            if (_lastAmmo!=null && _lastAmmo != _ammo)
            {
                _lastAmmo.GetComponent<Ammo>().isDestroyable=true;
                if(_lastAmmo.GetComponent<ExplosiveBase>().isExplode)
                    Destroy(_lastAmmo.gameObject);
            }
                
            _lastAmmo = _ammo;

            animator.SetBool("leave", true);
            _ammoRigidBody.isKinematic = false;
            _ammoRigidBody.useGravity = true;
            _ammoRigidBody.GetComponent<Collider>().enabled = true;
            _height = _ammoRigidBody.transform.position.y - _ammoStartY;

            _ammoRigidBody.transform.parent = null;
            _ammoRigidBody.AddForce(_ammoRigidBody.transform.up * launchPower * _height, ForceMode.VelocityChange);
            
        }
        OnLaunchedInvoke();
    }
    private void OnLaunchedInvoke()
    {
        OnLaunched?.Invoke();
    }
}
