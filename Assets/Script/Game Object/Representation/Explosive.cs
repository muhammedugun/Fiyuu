using UnityEngine;

/// <summary>
/// Patlay�c� ile ilgili g�revlerden sorumlu
/// </summary>
public class Explosive : MonoBehaviour
{
    [Tooltip("Patlaman�n particle effectid")]
    [SerializeField] private GameObject _explosionParticle;
    [Tooltip("Objenin par�alanabilir hali")]
    [SerializeField] private GameObject _smashableObject;
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
            Explode(collision);        
    }

    /// <summary>
    /// Patlat
    /// </summary>
    void Explode(Collision collision)
    {
        if(_smashableObject!=null)
        {
            Instantiate(_smashableObject, transform.position, Quaternion.identity);
        }        
        var surroundingObjects = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (var obj in surroundingObjects)
        {
            var rb = obj.GetComponent<Rigidbody>();
            if (rb == null) { continue; }
            rb.AddExplosionForce(_explosionForce * collision.relativeVelocity.magnitude, transform.position, _explosionRadius, 0.0f, ForceMode.Impulse);
            Instantiate(_explosionParticle, transform.position, Quaternion.identity);
            if(rb.TryGetComponent<IDamageable>(out IDamageable iDamageable))
            {
                var differentPosition = rb.transform.position - transform.position;
                iDamageable.Durability -= (_explosionForce * collision.relativeVelocity.magnitude*300) / differentPosition.magnitude;
            }
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
