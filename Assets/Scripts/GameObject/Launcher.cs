using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] private GameObject[] ammoTypePrefabs;
    [SerializeField] private Transform ammoSpawnPosition;

    [Header("Launcher")]
    [SerializeField] private Animator animator;
    [Tooltip("Fırlatma gücü")]
    [SerializeField] private float throwPower;
    [SerializeField] private MMF_Player loadFeedback;
    [SerializeField] private MMF_Player throwFeedback;

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
        _ammoStartY = ammoSpawnPosition.position.y;
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
            if (CheckAnimStateEmpty() && _ammo != null && _ammo.transform.position == ammoSpawnPosition.transform.position)
                Destroy(_ammo.gameObject);

            _ammo = Instantiate(ammoTypePrefabs[(int)_ammoManager.ammunition[0] - 1], ammoSpawnPosition);
            if (_lastAmmo == null)
                _lastAmmo = _ammo;
            _ammo.transform.position = ammoSpawnPosition.position;
            _ammo.transform.rotation = ammoSpawnPosition.rotation;
            _ammoRigidBody = _ammo.transform.GetChild(0).GetComponent<Rigidbody>();
        }
        
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
    /// Yükleme animasyonunu tetikle
    /// </summary>
    private void TriggerLoadAnim()
    {
        animator.SetBool("leave", false);
        animator.SetTrigger("load");
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

    private void HandleLoadAnimation()
    {
        if (_ammoManager.ammunition.Count > 0)
        {
            _lastClickTime = Time.time;
            TriggerLoadAnim();
            loadFeedback.PlayFeedbacks();
        }
    }

    private bool CanThrow()
    {
        return _ammoRigidBody.isKinematic && !animator.GetBool("leave") && Time.time > _lastClickTime + 0.4f;
    }

    private void ExecuteThrow()
    {
        loadFeedback.StopFeedbacks();
        throwFeedback.PlayFeedbacks();

        _collisionIconText.enabled = false;

        EnableTrailRenderer();

        HandlePreviousAmmo();

        SetAmmoProperties();

        ApplyThrowForce();

        OnThrowedInvoke();
    }

    private void EnableTrailRenderer()
    {
        _ammo.transform.GetChild(0).GetComponent<TrailRenderer>().enabled = true;
        _ammo.transform.GetChild(0).GetComponent<Ammo>().throwPos = _ammo.transform.position;
    }

    private void HandlePreviousAmmo()
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

    private void SetAmmoProperties()
    {
        animator.SetBool("leave", true);
        _ammoRigidBody.isKinematic = false;
        _ammoRigidBody.useGravity = true;
        _ammoRigidBody.GetComponent<Collider>().enabled = true;

        _height = _ammoRigidBody.transform.position.y - _ammoStartY;

        _ammo.transform.parent = null;
    }

    private void ApplyThrowForce()
    {
        _ammoRigidBody.AddForce(_ammoRigidBody.transform.right * throwPower * _height, ForceMode.VelocityChange);
    }

    private void OnThrowedInvoke()
    {
        EventBus.Publish(EventType.LauncherThrowed);
    }
}
