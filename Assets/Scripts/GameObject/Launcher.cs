// Refactor 23.08.24
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

/// <summary>
/// Mancınığın ana sınıfı
/// </summary>
public class Launcher : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] private GameObject[] _ammoTypePrefabs;
    [SerializeField] private Transform _ammoSpawnPosition;

    [Header("Launcher")]
    [SerializeField] private Animator _animator;
    [Tooltip("Fırlatma gücü")]
    [SerializeField] private float _throwPower;
    [SerializeField] private MMF_Player _loadFeedback;
    [SerializeField] private MMF_Player _throwFeedback;

    private AmmoManager _ammoManager;
    private TextMeshProUGUI _collisionIconText;
    private Vector3 _throwVelocity;
    private GameObject _ammo;
    private GameObject _lastAmmo;
    private Rigidbody _ammoRigidBody;
    private float _height;
    private float _ammoStartY;

    private void Start()
    {
        _ammoStartY = _ammoSpawnPosition.position.y;
        _ammoManager = FindObjectOfType<AmmoManager>();

        ThrowInputController.Started += Throw;
        _collisionIconText = GameObject.Find("/UI/Canvas/CollisionIcon").GetComponent<TextMeshProUGUI>();
        CreateAmmo();
    }


    private void OnDisable()
    {
        ThrowInputController.Started -= Throw;
    }

    /// <summary>
    /// Mühimmat oluşturur
    /// </summary>
    private void CreateAmmo()
    {
        
        if(_ammoManager.ammunition.Count>0)
        {
            // mevcut bir mühimmat varsa ve fırlatılmamışsa onu yok et
            if (CheckAnimStateEmpty() && _ammo != null && _ammo.transform.position == _ammoSpawnPosition.transform.position)
                Destroy(_ammo.gameObject);

            _ammo = Instantiate(_ammoTypePrefabs[(int)_ammoManager.ammunition[0] - 1], _ammoSpawnPosition);
            if (_lastAmmo == null)
                _lastAmmo = _ammo;
            _ammo.transform.position = _ammoSpawnPosition.position;
            _ammo.transform.rotation = _ammoSpawnPosition.rotation;
            _ammoRigidBody = _ammo.transform.GetChild(0).GetComponent<Rigidbody>();
        }
        
    }

    /// <summary>
    /// Empty animasyonunda mıyım diye kontrol eder
    /// </summary>
    /// <returns>Empty animasyonundaysa true, değilse false döndürür</returns>
    private bool CheckAnimStateEmpty()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Empty"))
            return true;
        else return false;
    }

    /// <summary>
    /// Yükleme animasyonunu tetikle
    /// </summary>
    private void TriggerLoadAnim()
    {
        _animator.SetBool("leave", false);
        _animator.SetTrigger("load");
    }

    float _lastClickTime;

    /// <summary>
    /// Fırlatma işlemini başlatır
    /// </summary>
    private void Throw()
    {
        if (CheckAnimStateEmpty())
        {
            HandleLoadAnimation();
        }
        else if (CanThrow())
        {
            ExecuteThrow();
        }
    }

    /// <summary>
    /// Mancınığın fırlatmaya hazırlık aşaması olan load aşamasının animasyonunu çalıştırır
    /// </summary>
    private void HandleLoadAnimation()
    {
        if (_ammoManager.ammunition.Count > 0)
        {
            _lastClickTime = Time.time;
            TriggerLoadAnim();
            _loadFeedback.PlayFeedbacks();
        }
    }

    /// <returns>Fırlatma durumuna uygun mu?</returns>
    private bool CanThrow()
    {
        return _ammoRigidBody.isKinematic && !_animator.GetBool("leave") && Time.time > _lastClickTime + 0.4f;
    }

    /// <summary>
    /// Fırlatmayı başlat
    /// </summary>
    private void ExecuteThrow()
    {
        _loadFeedback.StopFeedbacks();
        _throwFeedback.PlayFeedbacks();

        _collisionIconText.enabled = false;

        EnableTrailRenderer();

        DetermineLastAmmo();

        SetAmmoProperties();

        ApplyThrowForce();

        PublishOnThrowed();
    }

    /// <summary>
    /// Fırlatma sonrası için mühimmat arkasında çıkan izi etkinleştir
    /// </summary>
    private void EnableTrailRenderer()
    {
        _ammo.transform.GetChild(0).GetComponent<TrailRenderer>().enabled = true;
        _ammo.transform.GetChild(0).GetComponent<Ammo>().throwPos = _ammo.transform.position;
    }

    /// <summary>
    /// Şuan fırlatılacak olan mühimmatı son mühimmat olarak belirler
    /// </summary>
    private void DetermineLastAmmo()
    {
        if (_lastAmmo != null && _lastAmmo != _ammo)
        {
            if (_lastAmmo.transform.GetChild(0).GetComponent<ExplosiveBase>().isExplode)
                Destroy(_lastAmmo.gameObject);
            else
                _lastAmmo.transform.GetChild(0).GetComponent<TrailRenderer>().enabled = false;
        }
        _lastAmmo = _ammo;
    }

    /// <summary>
    /// Mühimmatın özelliklerini ayarlar
    /// </summary>
    private void SetAmmoProperties()
    {
        _animator.SetBool("leave", true);
        _ammoRigidBody.isKinematic = false;
        _ammoRigidBody.useGravity = true;
        _ammoRigidBody.GetComponent<Collider>().enabled = true;

        _height = _ammoRigidBody.transform.position.y - _ammoStartY;

        _ammo.transform.parent = null;
    }

    /// <summary>
    /// Fırlatma gücünü uygular
    /// </summary>
    private void ApplyThrowForce()
    {
        _ammoRigidBody.AddForce(_ammoRigidBody.transform.right * _throwPower * _height, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Fırlatıldı bilgisini event olarak yayar
    /// </summary>
    private void PublishOnThrowed()
    {
        EventBus.Publish(EventType.LauncherThrowed);
    }
}
