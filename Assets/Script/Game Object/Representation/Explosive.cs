using UnityEngine;

/// <summary>
/// Patlay�c� ile ilgili g�revlerden sorumlu
/// </summary>
public class Explosive : MonoBehaviour
{
    [SerializeField] private GameObject _particles, _smashableObject;
    [Tooltip("Patlaman�n yar��ap�")]
    [SerializeField] private float _explosionRadius = 5;
    [Tooltip("Patlaman�n g�c�")]
    [SerializeField] private float _explosionForce = 500;
    [Tooltip("Patlay�c� objenin dayan�kl�l���")]
    [SerializeField] private float _durability;
    

    private void OnCollisionEnter(Collision collision)
    {
        Damage(collision);
        if (CheckSmash())
            Explode();        
    }

    /// <summary>
    /// Patlat
    /// </summary>
    void Explode()
    {
        Instantiate(_smashableObject, transform.position, Quaternion.identity);
        var surroundingObjects = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (var obj in surroundingObjects)
        {
            var rb = obj.GetComponent<Rigidbody>();
            if (rb == null) { continue; }
            rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            //Instantiate(_particles, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Objenin dayan�kl�k de�erini azalt�r
    /// </summary>
    /// <param name="collision"></param>
    private void Damage(Collision collision)
    {
        var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        _durability -= collisionForce;
    }

    /// <summary>
    /// Par�alamann�n ger�ekle�ebilirli�ini kontrol eder
    /// </summary>
    /// <param name="collision"></param>
    private bool CheckSmash()
    {
        if (_durability <= 0) return true;
        else return false;

    }
}
