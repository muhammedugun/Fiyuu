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

    [SerializeField] private AmmoManager ammoSelectionUI;

    [SerializeField] private InputRange _inputRange;

    private TextMeshProUGUI _collisionIconText;
    private Vector3 _throwVelocity;
    private GameObject _ammo;
    private GameObject _lastAmmo;
    
    private Rigidbody _ammoRigidBody;
    /// <summary>
    /// Mühimmatın başlangıçtaki y pozisyonu
    /// </summary>
    private float _ammoStartY;


    private void OnDisable()
    {
        UnSubscribe();
    }

    private void Start()
    {
        Subscribe();
        _collisionIconText = GameObject.Find("/UI/Canvas/CollisionIcon").GetComponent<TextMeshProUGUI>();
        _ammoStartY = ammoSpawnPosition.position.y;
        CreateAmmo();
    }


    private void Subscribe()
    {
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
        
        if(ammoSelectionUI.ammunition.Count>0)
        {
            // mevcut bir mühimmat varsa ve fırlatılmamışsa onu yok et
            if (CheckAnimStateEmpty() && _ammo != null && _ammo.transform.position == ammoSpawnPosition.transform.position)
                Destroy(_ammo.gameObject);

            _ammo = Instantiate(ammoTypePrefabs[(int)ammoSelectionUI.ammunition[0] - 1], ammoSpawnPosition);
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
            loadFeedback.StopFeedbacks();
            throwFeedback.PlayFeedbacks();

            _collisionIconText.enabled = false;

            _ammo.transform.GetChild(0).GetComponent<TrailRenderer>().enabled = true;
            _ammo.transform.GetChild(0).GetComponent<Ammo>().throwPos = _ammo.transform.position;
            
            if (_lastAmmo!=null && _lastAmmo != _ammo)
            {
                if(_lastAmmo.transform.GetChild(0).GetComponent<ExplosiveBase>().isExplode)
                    Destroy(_lastAmmo.gameObject);
            }
                
            _lastAmmo = _ammo;

            animator.SetBool("leave", true);
            _ammoRigidBody.isKinematic = false;
            _ammoRigidBody.useGravity = true;
            _ammoRigidBody.GetComponent<Collider>().enabled = true;

            _ammo.transform.parent = null;
            _ammoRigidBody.AddForce(_ammoRigidBody.transform.right * throwPower, ForceMode.VelocityChange);
            OnThrowedInvoke();

        }
        
    }
    private void OnThrowedInvoke()
    {
        EventBus.Publish(EventType.LauncherThrowed);
    }
}
