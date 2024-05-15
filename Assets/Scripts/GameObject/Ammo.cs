// Refactor 12.03.24
using MoreMountains.Feedbacks;
using System.Collections;
using TMPro;
using UnityEngine;


/// <summary>
/// Mühimmat ile ilgili
/// </summary>
public class Ammo : ExplosiveBase
{
    [Header("Ammo")]
    [Tooltip("Mühimmatın maddesi")]
    public AmmoMatter matter;

    [Tooltip("Mühimmat düştükten sonra ne kadar yakınındaki düşmanları korkutsun?")]
    [SerializeField] private float scareRadius;
    [Tooltip("Trail Renderer'in görülmesini sağlayacak materyal")]
    [SerializeField] private Material visibleMaterial;
    [SerializeField] private float durabilityMultiplier = 100f;
    [SerializeField] private float massMultiplier = 10f;
    [SerializeField] private MMF_Player hitFeedback;
    [SerializeField] private MMF_Player destroyFeedback;
    [SerializeField] private float maxHitVolume;

    internal Vector3 throwPos;


    private bool isHit;
    private float _mass;

    /// <summary>
    /// Mühimmatın ilk çaprıştığı noktayı gösterecek olan X şeklindeki UI elemanı
    /// </summary>
    private TextMeshProUGUI _collisionIconText;
    private TrailRenderer _trailRenderer;
    private bool isCollisionShowed;

    public static int frameCount;
    
    private void Awake()
    {
        _massMultiplier = massMultiplier;
        _durabilityMultiplier = durabilityMultiplier;
    }
    protected override void Start()
    {
        base.Start();
        _trailRenderer = GetComponent<TrailRenderer>();
        _collisionIconText = GameObject.Find("/UI/Canvas/CollisionIcon").GetComponent<TextMeshProUGUI>();
        _mass = gameObject.GetComponent<Rigidbody>().mass;
    }

    GameObject moveble;
    float _moveableMass;
    private void OnCollisionEnter(Collision collision)
    {
        // İlk kez çarpışıldığı için ses efekti çalmakla ilgili işlemler yapılıyor
        if (!isHit )
        {
            isHit = true;
            _trailRenderer.material = visibleMaterial;

            bool isHitBuilding = collision.gameObject.TryGetComponent<Building>(out Building building);
            bool isHitEnemy = collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy);

            if (isHitBuilding || isHitEnemy)
            {
                
                var colForce = collision.impulse.magnitude / Time.fixedDeltaTime;
                if(colForce>600)
                {
                    hitFeedback.GetFeedbackOfType<MMF_Sound>().MaxVolume = maxHitVolume;
                    hitFeedback.GetFeedbackOfType<MMF_Sound>().MinVolume = maxHitVolume;
                }
                else if (colForce > 300)
                {
                    hitFeedback.GetFeedbackOfType<MMF_Sound>().MaxVolume = maxHitVolume*0.8f;
                    hitFeedback.GetFeedbackOfType<MMF_Sound>().MinVolume = maxHitVolume * 0.8f;
                }
                else if (colForce > 0)
                {
                    hitFeedback.GetFeedbackOfType<MMF_Sound>().MaxVolume = maxHitVolume * 0.6f;
                    hitFeedback.GetFeedbackOfType<MMF_Sound>().MinVolume = maxHitVolume * 0.6f;
                }
            }
            else
            {
                hitFeedback.GetFeedbackOfType<MMF_Sound>().MaxVolume = .2f;
                hitFeedback.GetFeedbackOfType<MMF_Sound>().MinVolume = .2f;
            }

            hitFeedback.PlayFeedbacks();
        }


        ShowCollisionIcon();
        RunResponseAnim(scareRadius);
        DoDamage(collision);
        if (CheckSmash())
        {
            Smash(collision);
            
            if (CheckWoodVersusStone(collision.gameObject, matter))
              AssignFragmentsMass();

            Explode(collision);
            isExplode = true;
            destroyFeedback.PlayFeedbacks();
        }

    }


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
        if(!isCollisionShowed)
        {
            isCollisionShowed = true;
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