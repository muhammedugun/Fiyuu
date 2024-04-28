using Blobcreate.ProjectileToolkit;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [Tooltip("Projectile için son transform noktası")]
    public Transform endTransform;

    [Header("Ammo")]
    [SerializeField] private GameObject[] ammoTypePrefabs;
    [SerializeField] private Transform ammoSpawnPosition;

    [Header("Launcher")]
    [SerializeField] private Animator animator;
    [Tooltip("Fırlatma gücü")]
    [SerializeField] private float throwPower;
    [SerializeField] private MMF_Player loadFeedback;
    [SerializeField] private MMF_Player throwFeedback;

    [SerializeField] private AmmoManager ammoSelectionUI;
    

    private TextMeshProUGUI _collisionIconText;
    private Vector3 _throwVelocity;
    private GameObject _ammo;
    private GameObject _lastAmmo;
    private InputRange _inputRange;
    private Rigidbody _ammoRigidBody;
    /// <summary>
    /// Mühimmatın başlangıçtaki y pozisyonu
    /// </summary>
    private float _ammoStartY;
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
        _collisionIconText = GameObject.Find("/UI/Canvas/CollisionIconText").GetComponent<TextMeshProUGUI>();
        _ammoStartY = ammoSpawnPosition.position.y;
        CreateAmmo();
    }
    
    /*
    private void LateUpdate()
    {
        
        if(_ammo!=null)
        {
          _height = _ammo.transform.position.y - _ammoStartY;
          if(_height>.02f)
          {
                if(animator.GetCurrentAnimatorStateInfo(0).IsName("Load") || animator.GetCurrentAnimatorStateInfo(0).IsName("Mirror Load"))
                    trajectoryPredictor.enabled = true;
                _throwVelocity = _ammoRigidBody.transform.right * throwPower;
                //trajectoryPredictor.Render(_ammoRigidBody.position, _throwVelocity, endTransform.position, trajectorySize);
          }
        }
    }*/
    

    private void Subscribe()
    {
        _inputRange = FindObjectOfType<InputRange>();
        _inputRange.started += Throw;
    }

    private void UnSubscribe()
    {
        _inputRange.started -= Throw;
    }


    /// <summary>
    /// Mühimmat oluşturur
    /// </summary>
    private void CreateAmmo()
    {
        // mevcut bir mühimmat varsa ve fırlatılmamışsa onu yok et
        if (CheckAnimStateEmpty() && _ammo != null && _ammo.transform.position == ammoSpawnPosition.transform.position)
            Destroy(_ammo.gameObject);

        _ammo = Instantiate(ammoTypePrefabs[(int)ammoSelectionUI.ammunition[0]-1], ammoSpawnPosition);
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
    /// Yükleme animasyonunu tetikle
    /// </summary>
    private void TriggerLoadAnim()
    {
        animator.SetBool("leave", false);
        animator.SetTrigger("load");
    }

    /// <summary>
    /// Fırlatma işlemini başlatır
    /// </summary>
    private void Throw()
    {
        if (CheckAnimStateEmpty())
        {
            TriggerLoadAnim();
            loadFeedback.PlayFeedbacks();
        }
        else if (_ammoRigidBody.isKinematic && !animator.GetBool("leave"))
        {
            Debug.LogWarning("Fırlatma Gerçekleşti");
            loadFeedback.StopFeedbacks();
            throwFeedback.PlayFeedbacks();

            _collisionIconText.enabled = false;

            _ammo.GetComponent<Ammo>().GetComponent<TrailRenderer>().enabled = true;
            _ammo.GetComponent<Ammo>().throwPos = _ammo.transform.position;
            
            if (_lastAmmo!=null && _lastAmmo != _ammo)
            {
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
            _ammoRigidBody.AddForce(_ammoRigidBody.transform.right * throwPower, ForceMode.VelocityChange);
            OnThrowedInvoke();

        }
        
    }
    private void OnThrowedInvoke()
    {
        EventBus.Publish(EventType.LauncherThrowed);
    }
}