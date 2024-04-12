// Refactor 12.03.24
using MoreMountains.Feedbacks;
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
    public MMF_Player inAirFeedback;

    [Tooltip("Mühimmat düştükten sonra ne kadar yakınındaki düşmanları korkutsun?")]
    [SerializeField] private float scareRadius;
    [Tooltip("Trail Renderer'in görülmesini sağlayacak materyal")]
    [SerializeField] private Material visibleMaterial;
    [SerializeField] private float durabilityMultiplier = 100f;
    [SerializeField] private float massMultiplier = 10f;
    [SerializeField] private MMF_Player hitFeedback;
    [SerializeField] private float maxHitVolume;

    internal Vector3 throwPos;

    internal bool isDestroyable;

    private bool isHit;

    /// <summary>
    /// Mühimmatın ilk çaprıştığı noktayı gösterecek olan X şeklindeki UI elemanı
    /// </summary>
    private TextMeshProUGUI _collisionIconText;
    private TrailRenderer _trailRenderer;
    private bool isCollisionShowed;
    
    private void Awake()
    {
        _massMultiplier = massMultiplier;
        _durabilityMultiplier = durabilityMultiplier;
    }
    protected override void Start()
    {
        base.Start();
        _trailRenderer = GetComponent<TrailRenderer>();
        _collisionIconText = GameObject.Find("/UI/Canvas/CollisionIconText").GetComponent<TextMeshProUGUI>();
    }

    GameObject moveble;
    float _moveableMass;
    private void OnCollisionEnter(Collision collision)
    {
        if (!isHit )
        {
            isHit = true;
            inAirFeedback.StopFeedbacks();

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
            /*
            if(collision.transform.CompareTag("Moveble"))
            {
                moveble = collision.gameObject;
                _moveableMass = moveble.GetComponent<Rigidbody>().mass;
                Invoke(nameof(Moveble), 0.1f);
            }*/

            hitFeedback.PlayFeedbacks();
        }
        
        ShowCollisionIcon();
        //ScareEnemies(scareRadius);
        DoDamage(collision);
        if (CheckSmash())
        {
            Smash(collision);
            GetComponent<Rigidbody>().isKinematic = true;
            _trailRenderer.material = visibleMaterial;
            Explode(collision);
            isExplode = true;
            if (isDestroyable)
            {
                Destroy(gameObject);
            }
        }
    }
    private void Moveble()
    {
        moveble.GetComponent<Rigidbody>().mass = 5f;
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
        // Dünya pozisyonunu ekran pozisyonuna çevir.
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
        // Ekran pozisyonunu RectTransform'ın yerel pozisyonuna çevir.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIElement.parent.GetComponent<RectTransform>(), screenPoint, null, out Vector2 localPoint);
        // Yeni pozisyonu ayarla.
        UIElement.anchoredPosition = localPoint;
    }


    /// <summary>
    /// Düşmanları korkut. Mühimmatın bulunduğu konumun çevresindeki düşmanların korkma animasyonlarını çalıştırır.
    /// </summary>
    private void ScareEnemies(float radius)
    {
        var hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.TryGetComponent<Enemy>(out var enemy);
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
    Iron,
    Steel,
    Fire,
    Ice,
    Explosion,
    Electric
}