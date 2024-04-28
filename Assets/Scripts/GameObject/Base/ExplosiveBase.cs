// Refactor 12.03.24
using UnityEngine;

/// <summary>
/// Patlayıcı nesne ile ilgili görevlerden sorumlu
/// </summary>
public abstract class ExplosiveBase : SmashableObjectBase
{
    [Header("Explosive")]
    [Tooltip("Particle effect olsun mu?")]
    [SerializeField] private bool _isParticleEffect;
    [Tooltip("Patlamanın particle effecti")]
    [SerializeField] private GameObject _explosionParticle;
    [Tooltip("Patlamanın yarıçapı")]
    [SerializeField] private float _explosionRadius = 5;
    [Tooltip("Patlamanın gücü")]
    [SerializeField] private float _explosionForce = 500;

    /// <summary>
    /// Patladı mı?
    /// </summary>
    internal bool isExplode;


    /// <summary>
    /// Patlat
    /// </summary>
    protected void Explode(Collision collision)
    {
        var surroundingObjects = Physics.OverlapSphere(transform.position, _explosionRadius);
        if (_isParticleEffect)
            Instantiate(_explosionParticle, transform.position, Quaternion.identity);
        foreach (var obj in surroundingObjects)
        {
            var rb = obj.GetComponent<Rigidbody>();
            if (rb == null) { continue; }
            rb.AddExplosionForce(_explosionForce * collision.relativeVelocity.magnitude, transform.position, _explosionRadius, 0.0f, ForceMode.Impulse);

            if (rb.TryGetComponent<SmashableObjectBase>(out SmashableObjectBase smashableObject))
            {
                var differentPosition = rb.transform.position - transform.position;
                smashableObject.durability -= (_explosionForce * collision.relativeVelocity.magnitude * 300) / differentPosition.magnitude;
            }
        }
        
    }

}
