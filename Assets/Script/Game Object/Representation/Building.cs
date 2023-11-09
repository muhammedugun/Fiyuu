using UnityEngine;

/// <summary>
/// Yap�(Bina) objelerini temsil eder
/// </summary>
public class Building : MonoBehaviour
{
    [Tooltip("Yap�n�n z�rh�")]
    [SerializeField] internal BuildingMatter armor;
    [Tooltip("Bu objenin par�alanabilir �rne�i")]
    [SerializeField] internal GameObject smashableObjectPrefab;
    /// <summary>
    /// Objenin hacmi. Yani objenin uzayda kaplad��� alan�n boyutu.
    /// </summary>
    private float _volumeSize;
    /// <summary>
    /// Objenin dayan�kl�l���
    /// </summary>
    internal float durability;

    /// <summary>
    /// Z�rh dayan�kl�klar�
    /// </summary>
    private int[,] armorStrengths = new int[4, 8]
    {
        // M�himmat z�rh t�rleri (s�tunlar).
        // Ah�ap=1, ta�=2, demir=3, �elik=4, ate�=5, buz=6, patlama=7, elektrik=8
        // Bina z�rh t�rleri (sat�rlar)
        { 0,0,0,0,0,0,0,1 }, // Ah�ap
        { 1,0,0,0,1,0,0,1 }, // Ta�
        { 1,1,0,0,1,0,0,0 }, // Demir
        { 1,1,1,1,1,1,1,0 }  // �elik
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
    /// Objenin a��rl���n� atar
    /// </summary>
    private void AssignMass(Rigidbody rb, float volumeSize)
    {
        if(volumeSize<=0)
        {
            Debug.LogError("Yap�n�n hacmi atanmam�� ya da hacmi s�f�r");
        }
        else
        {
            rb.mass = (int)armor * (volumeSize / 20);
        }
        
    }

    /// <summary>
    /// Objenin dayan�kl�l���n� atar
    /// </summary>
    private void AssignDurability()
    {
        durability = (int)armor *200; // de�eri �imdilik temsili koyulmu�tur
    }


    /// <summary>
    /// Objenin dayan�kl�k de�erini azalt�r
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
    /// Par�alamay� ger�ekle�tirir
    /// </summary>
    /// <param name="smashableObject">Objenin par�alanabilir halinin �rne�i</param>
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
    /// Par�alaman�n ger�ekle�ebilirli�ini kontrol eder
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


