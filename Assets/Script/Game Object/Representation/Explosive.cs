using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Fracture))]
/// <summary>
/// Patlay�c� ile ilgili g�revlerden sorumlu
/// </summary>
public class Explosive : MonoBehaviour, ISmashable
{
    [Tooltip("Particle effect olsun mu?")]
    [SerializeField] private bool _isParticleEffect;
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

    private Fracture fracture;

    public float Durability { get { return _durability; } set { _durability = value; } }

    private void OnCollisionEnter(Collision collision)
    {
        DoDamage(collision);
        if (CheckSmash())
            Explode(collision);
    }

    /// <summary>
    /// Patlat
    /// </summary>
    void Explode(Collision collision)
    {
        Smash(collision);
        var surroundingObjects = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (var obj in surroundingObjects)
        {
            var rb = obj.GetComponent<Rigidbody>();
            if (rb == null) { continue; }
            rb.AddExplosionForce(_explosionForce * collision.relativeVelocity.magnitude, transform.position, _explosionRadius, 0.0f, ForceMode.Impulse);
            if(_isParticleEffect)
                Instantiate(_explosionParticle, transform.position, Quaternion.identity);
            if (rb.TryGetComponent<IDamageable>(out IDamageable iDamageable))
            {
                var differentPosition = rb.transform.position - transform.position;
                iDamageable.Durability -= (_explosionForce * collision.relativeVelocity.magnitude * 300) / differentPosition.magnitude;
            }
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Objenin dayan�kl�k de�erini azalt�r
    /// </summary>
    /// <param name="collision"></param>
    public void DoDamage(Collision collision)
    {
        var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        _durability -= collisionForce;
    }

    /// <summary>
    /// Par�alamann�n ger�ekle�ebilirli�ini kontrol eder
    /// </summary>
    /// <param name="collision"></param>
    public bool CheckSmash()
    {
        if (_durability <= 0) return true;
        else return false;

    }

    public void Smash(Collision collision)
    {
        if (collision.contactCount > 0)
        {
            // Collision force must exceed the minimum force (F = I / T)
            var contact = collision.contacts[0];
            fracture.callbackOptions.CallOnFracture(contact.otherCollider, gameObject, contact.point);
            fracture.ComputeFracture();
        }
    }
}
