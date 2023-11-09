using UnityEngine;

/// <summary>
/// Yapý(Bina) objelerini temsil eder
/// </summary>
public class Building : MonoBehaviour
{
    [Tooltip("Yapýnýn zýrhý")]
    [SerializeField] internal BuildingMatter armor;
    [Tooltip("Bu objenin parçalanabilir örneði")]
    [SerializeField] internal GameObject smashableObjectPrefab;
    /// <summary>
    /// Objenin hacmi. Yani objenin uzayda kapladýðý alanýn boyutu.
    /// </summary>
    private float _volumeSize;
    /// <summary>
    /// Objenin dayanýklýlýðý
    /// </summary>
    internal float durability;

    /// <summary>
    /// Zýrh dayanýklýklarý
    /// </summary>
    private int[,] armorStrengths = new int[4, 8]
    {
        // Mühimmat zýrh türleri (sütunlar).
        // Ahþap=1, taþ=2, demir=3, çelik=4, ateþ=5, buz=6, patlama=7, elektrik=8
        // Bina zýrh türleri (satýrlar)
        { 0,0,0,0,0,0,0,1 }, // Ahþap
        { 1,0,0,0,1,0,0,1 }, // Taþ
        { 1,1,0,0,1,0,0,0 }, // Demir
        { 1,1,1,1,1,1,1,0 }  // Çelik
    };

    private void Start()
    {
        AssignDurability();
        AssignVolume(gameObject.GetComponent<Renderer>(), ref _volumeSize);
        AssignMass(gameObject.GetComponent<Rigidbody>(), _volumeSize);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DoDamage(collision);
        if (CheckSmash()) Smash(smashableObjectPrefab);
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
        if(volumeSize<=0)
        {
            Debug.LogError("Yapýnýn hacmi atanmamýþ ya da hacmi sýfýr");
        }
        else
        {
            rb.mass = (int)armor * (volumeSize / 20);
        }
        
    }

    /// <summary>
    /// Objenin dayanýklýlýðýný atar
    /// </summary>
    private void AssignDurability()
    {
        durability = (int)armor *200; // deðeri þimdilik temsili koyulmuþtur
    }


    /// <summary>
    /// Objenin dayanýklýk deðerini azaltýr
    /// </summary>
    /// <param name="collision"></param>
    private void DoDamage(Collision collision)
    {
        if(collision.transform.CompareTag("Ammo"))
        {
            var ammoArmor = collision.gameObject.GetComponent<Ammo>().armor;
            if (armorStrengths[(int)armor - 1, (int)ammoArmor - 1] == 0)
            {
                Debug.Log("Damage!");
                var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
                durability -= collisionForce;
            }
        }
        else
        {
            var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
            durability -= collisionForce;
        }
    }

    /// <summary>
    /// Parçalamayý gerçekleþtirir
    /// </summary>
    /// <param name="smashableObject">Objenin parçalanabilir halinin örneði</param>
    private void Smash(GameObject smashableObject)
    {
        var initializedSmashableObject = Instantiate(smashableObject, gameObject.transform.position, smashableObject.transform.rotation);
        for(int i=0; i < initializedSmashableObject.transform.childCount; i++)
        {
            var child = initializedSmashableObject.transform.GetChild(i);
            float volumeSize=0;
            AssignVolume(child.GetComponent<Renderer>(), ref volumeSize);
            var rb = child.GetComponent<Rigidbody>();
            AssignMass(rb, volumeSize);
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Parçalamanýn gerçekleþebilirliðini kontrol eder
    /// </summary>
    /// <param name="collision"></param>
    private bool CheckSmash()
    {
        if (durability <= 0) return true;
        else return false;

    }

}

/// <summary>
/// Madde
/// </summary>
public enum BuildingMatter
{
    Wood = 1,
    Stone,
    Iron,
    Steel
}


