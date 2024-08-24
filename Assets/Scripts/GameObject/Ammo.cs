// Refactor 23.08.24
using MoreMountains.Feedbacks;
using System.Collections;
using TMPro;
using UnityEngine;


/// <summary>
/// Mühimmatların ana sınıfıdır
/// </summary>
public class Ammo : ExplosiveBase
{
    [Header("Ammo")]
    [Tooltip("Mühimmatın maddesi")]
    public AmmoMatter matter;

    public float durabilityMultiplier = 100f;
    public float massMultiplier = 10f;

    internal Vector3 throwPos;

    [Tooltip("Mühimmat düştükten sonra ne kadar yakınındaki düşmanları korkutsun?")]
    [SerializeField] private float _scareRadius;
    [Tooltip("Trail Renderer'in görülmesini sağlayacak materyal")]
    [SerializeField] private Material _visibleMaterial;
    [SerializeField] private MMF_Player _hitFeedback;
    [SerializeField] private MMF_Player _destroyFeedback;
    [SerializeField] private float _maxHitVolume;

    /// <summary>
    /// Mühimmatın ilk çaprıştığı noktayı gösterecek olan X şeklindeki UI elemanı
    /// </summary>
    private TextMeshProUGUI _collisionIconText;
    private TrailRenderer _trailRenderer;
    private bool _isHit;
    private float _mass;
    private bool _isCollisionShowed;

    private void Awake()
    {
        base._massMultiplier = massMultiplier;
        base._durabilityMultiplier = durabilityMultiplier;
    }
    protected override void Start()
    {
        base.Start();
        _trailRenderer = GetComponent<TrailRenderer>();
        _collisionIconText = GameObject.Find("/UI/Canvas/CollisionIcon").GetComponent<TextMeshProUGUI>();
        _mass = gameObject.GetComponent<Rigidbody>().mass;
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        // İlk kez çarpışıldığı için ses efekti çalmakla ilgili işlemler yapılıyor
        if (!_isHit )
        {
            _isHit = true;
            _trailRenderer.material = _visibleMaterial;

            bool isHitBuilding = collision.gameObject.TryGetComponent<Building>(out Building building);
            bool isHitEnemy = collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy);

            if (isHitBuilding || isHitEnemy)
            {
                
                var colForce = collision.impulse.magnitude / Time.fixedDeltaTime;
                if(colForce>600)
                {
                    _hitFeedback.GetFeedbackOfType<MMF_Sound>().MaxVolume = _maxHitVolume;
                    _hitFeedback.GetFeedbackOfType<MMF_Sound>().MinVolume = _maxHitVolume;
                }
                else if (colForce > 300)
                {
                    _hitFeedback.GetFeedbackOfType<MMF_Sound>().MaxVolume = _maxHitVolume*0.8f;
                    _hitFeedback.GetFeedbackOfType<MMF_Sound>().MinVolume = _maxHitVolume * 0.8f;
                }
                else if (colForce > 0)
                {
                    _hitFeedback.GetFeedbackOfType<MMF_Sound>().MaxVolume = _maxHitVolume * 0.6f;
                    _hitFeedback.GetFeedbackOfType<MMF_Sound>().MinVolume = _maxHitVolume * 0.6f;
                }
            }
            else
            {
                _hitFeedback.GetFeedbackOfType<MMF_Sound>().MaxVolume = .2f;
                _hitFeedback.GetFeedbackOfType<MMF_Sound>().MinVolume = .2f;
            }

            _hitFeedback.PlayFeedbacks();
        }


        ShowCollisionIcon();
        RunResponseAnim(_scareRadius);
        DoDamage(collision);
        if (CheckSmash())
        {
            Smash(collision);
            
            if (CheckWoodVersusStone(collision.gameObject, matter))
              AssignFragmentsMass();

            Explode(collision);
            isExplode = true;
            _destroyFeedback.PlayFeedbacks();
        }

    }

    /// <summary>
    /// Mühimmatın parçalandıktan sonraki parçalarına ağırlık ataması yapar
    /// </summary>
    void AssignFragmentsMass()
    {
        var fragmentsParent = transform.parent.GetChild(2).gameObject;

        for (int i = 0; i < fragmentsParent.transform.childCount; i++)
        {
            fragmentsParent.transform.GetChild(i).GetComponent<Rigidbody>().mass = _mass / fragmentsParent.transform.childCount;
        }

    }

    /// <summary>
    /// Odun mühimmata karşı taş yapı kontrolü
    /// </summary>
    /// <returns>Bu mühimmat odunsa ve collision'da taş yapıysa true döndürür</returns>
    public static bool CheckWoodVersusStone(GameObject triggerObject, AmmoMatter matter)
    {
        if (triggerObject.CompareTag("Building") && matter == AmmoMatter.Wood)
        {
            var building = triggerObject.GetComponent<Building>();
            if (building != null && building.armor == BuildingMatter.Stone)
            {
                return true;
            }
        }
        return false;
    }

    public override void DoDamage(Collision collision, float damageMultiplier = 1f)
    {
        if (CheckWoodVersusStone(collision.gameObject, matter))
        {
            gameObject.GetComponent<Rigidbody>().mass = _mass;
            durability = 0f;
        }
        else
        {
            base.DoDamage(collision, damageMultiplier);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CheckWoodVersusStone(other.gameObject, matter))
            gameObject.GetComponent<Rigidbody>().mass = 0f;
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckWoodVersusStone(other.gameObject, matter))
            gameObject.GetComponent<Rigidbody>().mass = _mass;
    }

    /// <summary>
    /// Çarpışılan noktada "X" işareti gösterir. Bu sayede oyuncu attığı mühimmatın nerede ilk çarpışma yaşadığını anlar
    /// </summary>
    private void ShowCollisionIcon()
    {
        if(!_isCollisionShowed)
        {
            _isCollisionShowed = true;
            _collisionIconText.enabled = true;
            MoveUIToWorldPos(_collisionIconText.rectTransform, transform.position);
        }
    }

    /// <summary>
    /// UI elemanını verilen dünya pozisyonuna denk gelecek şekilde ayarlar
    /// </summary>
    /// <param name="UIElement"></param>
    /// <param name="worldPos"></param>
    private void MoveUIToWorldPos(RectTransform UIElement, Vector3 worldPos)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIElement.parent.GetComponent<RectTransform>(), screenPoint, null, out Vector2 localPoint);

        UIElement.anchoredPosition = localPoint;
    }


    /// <summary>
    /// Mühimmatın çarpması sonucu düşmanların tepki verme animasyonunu çalıştır.
    /// </summary>
    private void RunResponseAnim(float radius)
    {
        var hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            bool isThereEnemy = hitCollider.TryGetComponent<Enemy>(out var enemy);
            if(isThereEnemy)
                enemy.gameObject.GetComponent<Animator>().SetTrigger("Response");
        }
    }

}

/// <summary>
/// Mühimmatın maddesi
/// </summary>
public enum AmmoMatter
{
    Wood = 1,
    Stone,
    Fire,
    Explosion
}