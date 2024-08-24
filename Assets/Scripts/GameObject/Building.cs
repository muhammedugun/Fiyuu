// Refactor 23.08.24
using MoreMountains.Feedbacks;
using UnityEngine;

/// <summary>
/// Yap�(Bina) objelerini temsil eder
/// </summary>
public class Building : SmashableObjectBase
{
    [Tooltip("Yap�n�n z�rh�")]
    public BuildingMatter armor;

    /// <summary>
    /// Yap� z�rh�na g�re dayan�kl�l�k de�erlerini saklayan dizi. 
    /// <para>  �rnek: 0. index 1. yap� maddesi olan ah�apa denk gelir. </para>
    /// </summary>
    public float[] armorDurabilitiy = new float[2] { 300f, 400f };

    /// <summary>
    /// Z�rh�n g��l� y�nleri. Z�rh�n neye dayan�kl� olup neye dayan�kl� olmad���. 
    /// <para> Sat�rlar: Bina z�rh�, S�tunlar: m�himmat z�rh� </para>
    /// <para> Sonucun 1 olmas� binan�n bu m�himmata kar�� dayan�kl� oldu�unu g�sterir</para>
    /// </summary>
    public static int[,] armorStrengths = new int[2, 4]
    {
        // M�himmat maddesi t�rleri (s�tunlar).
        // Ah�ap=0, ta�=1, ate�=2, patlay�c�=3
        // Bina maddesi t�rleri (sat�rlar)
        { 0,0,0,0}, // Ah�ap
        { 1,0,1,0}, // Ta�
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
    /// Ba�lang�� feedback'lerini atlar
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
/// Yap�n�n maddesi(z�rh�) 
/// </summary>
public enum BuildingMatter
{
    Wood = 1,
    Stone
}