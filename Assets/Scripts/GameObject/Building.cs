// Refactor 12.03.24
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Yapý(Bina) objelerini temsil eder
/// </summary>
public class Building : SmashableObjectBase
{
    [Tooltip("Yapýnýn zýrhý")]
    public BuildingMatter armor;

    [SerializeField] private MMF_Player damageFeedbacks;
    [SerializeField] private MMF_Player destroyFeedbacks;
    [SerializeField] private MMF_Player beginningFeedback;
    [SerializeField] private MMF_Player skipBeginningFeedback;
    /// <summary>
    /// Yapý zýrhýna göre dayanýklýlýk deðerlerini saklayan dizi. 
    /// <para>  Örnek: 0. index 1. yapý maddesi olan ahþapa denk gelir. </para>
    /// </summary>
    public float[] armorDurabilitiy = new float[4] { 100f, 200f, 600f, 800f };


    /// <summary>
    /// Zýrhýn güçlü yönleri. Zýrhýn neye dayanýklý olup neye dayanýklý olmadýðý. 
    /// <para> Satýrlar: Bina maddesi(zýrhý), Sütunlar: mühimmat maddesi </para>
    /// </summary>
    private int[,] _armorStrengths = new int[4, 8]
    {
        // Mühimmat maddesi türleri (sütunlar).
        // Ahþap=1, taþ=2, demir=3, çelik=4, ateþ=5, buz=6, patlama=7, elektrik=8
        // Bina maddesi türleri (satýrlar)
        { 0,0,0,0,0,0,0,1 }, // Ahþap
        { 1,0,0,0,1,0,0,1 }, // Taþ
        { 1,1,0,0,1,0,0,0 }, // Demir
        { 1,1,1,1,1,1,1,0 }  // Çelik
    };


    private void Awake()
    {
        _massMultiplier = (int)armor;
        _durabilityMultiplier = armorDurabilitiy[(int)armor - 1];
    }

    protected override void Start()
    {
        ControllerManager.action.InLevel.Attack.started += SkipBeginning;
        MMFloatingTextSpawner floatingTextSpawner = FindObjectOfType<MMFloatingTextSpawner>();
        base.Start();
    }

    private void OnCollisionEnter(Collision collision)
    {
        DoDamage(collision);
        if (CheckSmash())
        {
            Smash(collision);
        }
    }

    public void SkipBeginning(InputAction.CallbackContext context)
    {
        ControllerManager.action.InLevel.Attack.started -= SkipBeginning;
        beginningFeedback.StopFeedbacks();
        skipBeginningFeedback.PlayFeedbacks();
        
    }


    public override void DoDamage(Collision collision, float damageMultiplier = 1f)
    {
        var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        if (durability > 0 && collisionForce / _rigidbody.mass > 200f)
        {
            if (collision.transform.CompareTag("Ammo"))
            {
                var ammo = collision.gameObject.GetComponent<Ammo>();
                if(ammo!=null)
                {
                    damageFeedbacks?.PlayFeedbacks();
                    var ammoArmor = ammo.matter;

                    if (_armorStrengths[(int)armor - 1, (int)ammoArmor - 1] == 0)
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
        
        destroyFeedbacks.PlayFeedbacks(this.transform.position, ScoreManager.CalculateScore(_volumeSize, armor));

    }

   
}



/// <summary>
/// Yapýnýn maddesi(zýrhý) 
/// </summary>
public enum BuildingMatter
{
    Wood = 1,
    Stone,
    Iron,
    Steel
}