// Refactor 23.08.24
using MoreMountains.Feedbacks;
using UnityEngine;

/// <summary>
/// Yapý(Bina) objelerini temsil eder
/// </summary>
public class Building : SmashableObjectBase
{
    [Tooltip("Yapýnýn zýrhý")]
    public BuildingMatter armor;

    /// <summary>
    /// Yapý zýrhýna göre dayanýklýlýk deðerlerini saklayan dizi. 
    /// <para>  Örnek: 0. index 1. yapý maddesi olan ahþapa denk gelir. </para>
    /// </summary>
    public float[] armorDurabilitiy = new float[2] { 300f, 400f };

    /// <summary>
    /// Zýrhýn güçlü yönleri. Zýrhýn neye dayanýklý olup neye dayanýklý olmadýðý. 
    /// <para> Satýrlar: Bina zýrhý, Sütunlar: mühimmat zýrhý </para>
    /// <para> Sonucun 1 olmasý binanýn bu mühimmata karþý dayanýklý olduðunu gösterir</para>
    /// </summary>
    public static int[,] armorStrengths = new int[2, 4]
    {
        // Mühimmat maddesi türleri (sütunlar).
        // Ahþap=0, taþ=1, ateþ=2, patlayýcý=3
        // Bina maddesi türleri (satýrlar)
        { 0,0,0,0}, // Ahþap
        { 1,0,1,0}, // Taþ
    };

    [SerializeField] private MMF_Player _damageFeedbacks;
    [SerializeField] private MMF_Player _destroyFeedbacks;
    [SerializeField] private MMF_Player _skipBeginningFeedback;

    private void Awake()
    {
        _massMultiplier = (int)armor;
        _durabilityMultiplier = armorDurabilitiy[(int)armor - 1];
    }

    protected override void Start()
    {     
        MMFloatingTextSpawner floatingTextSpawner = FindObjectOfType<MMFloatingTextSpawner>();
        base.Start();
    }

    private void OnEnable()
    {
        EventBus.Subscribe(EventType.FirstClickInLevel, SkipBeginning);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DoDamage(collision);
        if (CheckSmash())
        {
            Smash(collision);
        }    
    }
    /// <summary>
    /// Baþlangýç feedback'lerini atlar
    /// </summary>
    public void SkipBeginning()
    {
        _skipBeginningFeedback.PlayFeedbacks();
        EventBus.Unsubscribe(EventType.FirstClickInLevel, SkipBeginning);
    }

    public override void DoDamage(Collision collision, float damageMultiplier = 1f)
    {
        var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        if (durability > 0 && collisionForce > damageSensitivity)
        {
            if (collision.transform.CompareTag("Ammo"))
            {
                var ammo = collision.gameObject.GetComponent<Ammo>();
                if(ammo!=null)
                {
                    _damageFeedbacks?.PlayFeedbacks();
                    var ammoArmor = ammo.matter;
                    if (armorStrengths[(int)armor - 1, (int)ammoArmor - 1] == 0)
                    {
                        base.DoDamage(collision);
                    }
                }
                
            }
            else if (collision.gameObject.layer != LayerMask.NameToLayer("Node"))
            {
                base.DoDamage(collision);
            }
        }
    }

    public override void Smash(Collision collision)
    {
        base.Smash(collision);
        EventBus<float, BuildingMatter>.Publish(EventType.BuildSmashed, _volumeSize, armor);
        EventBus.Publish(EventType.BuildSmashed);
        
        _destroyFeedbacks.PlayFeedbacks(transform.position, ScoreManager.CalculateScore(_volumeSize, armor));

    }  
}

/// <summary>
/// Yapýnýn maddesi(zýrhý) 
/// </summary>
public enum BuildingMatter
{
    Wood = 1,
    Stone
}