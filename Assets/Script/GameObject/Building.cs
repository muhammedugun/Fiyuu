using MoreMountains.Feedbacks;
using System;
using UnityEngine;

/// <summary>
/// Yapý(Bina) objelerini temsil eder
/// </summary>
public class Building : MonoBehaviour, ISmashable
{
    [SerializeField] private MMF_Player damageFeedbacks, beginningFeedbacks;
    public static event Action<float, BuildingMatter> OnBuildSmashed; // float yapýnýn hacmi, BuildingMatter yapýnýn zýrhý
    public float Durability { get { return _durability; } set { _durability = value; } }

    [Tooltip("Yapýnýn zýrhý")]
    [SerializeField] internal BuildingMatter armor;
    private Fracture fracture;

    private float _durability;
    /// <summary>
    /// Objenin hacmi. Yani objenin uzayda kapladýðý alanýn boyutu.
    /// </summary>
    private float _volumeSize;

    /// <summary>
    /// Zýrh dayanýklýklarý. Zýrhýn neye dayanýklý olup neye dayanýklý olmadýðý. Satýrlar: Bina maddesi(zýrhý), Sütunlar: mühimmat maddesi
    /// </summary>
    private int[,] armorStrengths = new int[4, 8]
    {
        // Mühimmat maddesi türleri (sütunlar).
        // Ahþap=1, taþ=2, demir=3, çelik=4, ateþ=5, buz=6, patlama=7, elektrik=8
        // Bina maddesi türleri (satýrlar)
        { 0,0,0,0,0,0,0,1 }, // Ahþap
        { 1,0,0,0,1,0,0,1 }, // Taþ
        { 1,1,0,0,1,0,0,0 }, // Demir
        { 1,1,1,1,1,1,1,0 }  // Çelik
    };


    internal float[] _matterDurabilitiy = new float[4] { 2000f, 40000f, 60000f, 80000f};

    /// <summary>
    /// Parçalanma gerçekleþti mi?
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
    /// Objenin aðýrlýðýný atar
    /// </summary>
    private void AssignMass(Rigidbody rb, float volumeSize)
    {
        if (volumeSize <= 0)
        {
            Debug.LogError("Yapýnýn hacmi atanmamýþ ya da hacmi sýfýr");
        }
        else
        {
            rb.mass = (int)armor * (volumeSize);
        }

    }

    /// <summary>
    /// Objenin dayanýklýlýðýný atar
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
    /// Parçalamayý gerçekleþtirir
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
    /// Parçalamanýn gerçekleþebilirliðini kontrol eder
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
/// Yapýnýn maddesi(zýrhý) 
/// </summary>
public enum BuildingMatter
{
    Wood = 1,
    Stone,
    Iron,
    Steel
}


