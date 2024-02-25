using MoreMountains.Feedbacks;
using System;
using UnityEngine;

/// <summary>
/// Yap�(Bina) objelerini temsil eder
/// </summary>
public class Building : MonoBehaviour, ISmashable
{
    [SerializeField] private MMF_Player damageFeedbacks, beginningFeedbacks;
    public static event Action<float, BuildingMatter> OnBuildSmashed; // float yap�n�n hacmi, BuildingMatter yap�n�n z�rh�
    public float Durability { get { return _durability; } set { _durability = value; } }

    [Tooltip("Yap�n�n z�rh�")]
    [SerializeField] internal BuildingMatter armor;
    private Fracture fracture;

    private float _durability;
    /// <summary>
    /// Objenin hacmi. Yani objenin uzayda kaplad��� alan�n boyutu.
    /// </summary>
    private float _volumeSize;

    /// <summary>
    /// Z�rh dayan�kl�klar�. Z�rh�n neye dayan�kl� olup neye dayan�kl� olmad���. Sat�rlar: Bina maddesi(z�rh�), S�tunlar: m�himmat maddesi
    /// </summary>
    private int[,] armorStrengths = new int[4, 8]
    {
        // M�himmat maddesi t�rleri (s�tunlar).
        // Ah�ap=1, ta�=2, demir=3, �elik=4, ate�=5, buz=6, patlama=7, elektrik=8
        // Bina maddesi t�rleri (sat�rlar)
        { 0,0,0,0,0,0,0,1 }, // Ah�ap
        { 1,0,0,0,1,0,0,1 }, // Ta�
        { 1,1,0,0,1,0,0,0 }, // Demir
        { 1,1,1,1,1,1,1,0 }  // �elik
    };


    internal float[] _matterDurabilitiy = new float[4] { 2000f, 40000f, 60000f, 80000f};

    /// <summary>
    /// Par�alanma ger�ekle�ti mi?
    /// </summary>
    private bool _isSmash;
    private void Start()
    {
        fracture = GetComponent<Fracture>();
        AssignDurability();
        AssignVolume(GetComponentInChildren<Renderer>(), ref _volumeSize);
        AssignMass(gameObject.GetComponent<Rigidbody>(), _volumeSize);
        beginningFeedbacks?.PlayFeedbacks();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isSmash)
        {
            DoDamage(collision);
            if (CheckSmash())
            {
                _isSmash = true;
                Smash(collision);
            }
        }
    }

    /// <summary>
    /// Objenin hacmini atar
    /// </summary>
    private void AssignVolume(Renderer renderer, ref float volumeSize)
    {
        Bounds bounds = renderer.bounds;
        volumeSize = bounds.size.x * bounds.size.y * bounds.size.z;
    }

    /// <summary>
    /// Objenin a��rl���n� atar
    /// </summary>
    private void AssignMass(Rigidbody rb, float volumeSize)
    {
        if (volumeSize <= 0)
        {
            Debug.LogError("Yap�n�n hacmi atanmam�� ya da hacmi s�f�r");
        }
        else
        {
            rb.mass = (int)armor * (volumeSize);
        }

    }

    /// <summary>
    /// Objenin dayan�kl�l���n� atar
    /// </summary>
    private void AssignDurability()
    {
        _durability = _matterDurabilitiy[(int)armor - 1];
    }

    public void DoDamage(Collision collision)
    {

        if (collision.transform.CompareTag("Ammo"))
        {
            damageFeedbacks?.PlayFeedbacks();
            var ammo = collision.gameObject.GetComponent<Ammo>();
            var ammoArmor = ammo.matter;
            if (armorStrengths[(int)armor - 1, (int)ammoArmor - 1] == 0)
            {
                var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
                Debug.Log("collisionForce: " + collisionForce);
                _durability -= collisionForce * ammo.power[(int)ammoArmor - 1];
                Debug.Log("damage: " + collisionForce * ammo.power[(int)ammoArmor - 1]);
            }
        }
        else if(collision.gameObject.layer!=LayerMask.NameToLayer("Node"))
        {
            Debug.Log(collision.impulse.magnitude);
            var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
            _durability -= collisionForce;
        }
    }


    /// <summary>
    /// Par�alamay� ger�ekle�tirir
    /// </summary>
    public void Smash(Collision collision)
    {
        if (collision.contactCount > 0)
        {
            // Collision force must exceed the minimum force (F = I / T)
            var contact = collision.contacts[0];
            fracture.callbackOptions.CallOnFracture(contact.otherCollider, gameObject, contact.point);
            fracture.ComputeFracture();
        }

        OnBuildSmashed?.Invoke(_volumeSize, armor);
    }

    /// <summary>
    /// Par�alaman�n ger�ekle�ebilirli�ini kontrol eder
    /// </summary>
    /// <param name="collision"></param>
    public bool CheckSmash()
    {
        if (_durability <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}



/// <summary>
/// Yap�n�n maddesi(z�rh�) 
/// </summary>
public enum BuildingMatter
{
    Wood = 1,
    Stone,
    Iron,
    Steel
}


