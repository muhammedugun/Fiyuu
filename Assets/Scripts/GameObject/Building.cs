// Refactor 12.03.24
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Yap�(Bina) objelerini temsil eder
/// </summary>
public class Building : SmashableObjectBase
{
    [Tooltip("Yap�n�n z�rh�")]
    public BuildingMatter armor;

    [SerializeField] private MMF_Player damageFeedbacks;
    [SerializeField] private MMF_Player destroyFeedbacks;
    [SerializeField] private MMF_Player beginningFeedback;
    [SerializeField] private MMF_Player skipBeginningFeedback;
    /// <summary>
    /// Yap� z�rh�na g�re dayan�kl�l�k de�erlerini saklayan dizi. 
    /// <para>  �rnek: 0. index 1. yap� maddesi olan ah�apa denk gelir. </para>
    /// </summary>
    public float[] armorDurabilitiy = new float[4] { 100f, 200f, 600f, 800f };


    /// <summary>
    /// Z�rh�n g��l� y�nleri. Z�rh�n neye dayan�kl� olup neye dayan�kl� olmad���. 
    /// <para> Sat�rlar: Bina z�rh�, S�tunlar: m�himmat z�rh� </para>
    /// </summary>
    public static int[,] armorStrengths = new int[2, 4]
    {
        // M�himmat maddesi t�rleri (s�tunlar).
        // Ah�ap=0, ta�=1, ate�=2, patlay�c�=3
        // Bina maddesi t�rleri (sat�rlar)
        { 0,0,0,0}, // Ah�ap
        { 1,0,1,0}, // Ta�
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
        
        destroyFeedbacks.PlayFeedbacks(this.transform.position, ScoreManager.CalculateScore(_volumeSize, armor));

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