using UnityEngine;

/// <summary>
/// Patlayýcý ile ilgili görevlerden sorumlu
/// </summary>
public class Explosive : MonoBehaviour, ISmashable
{
    [Tooltip("Particle effect olsun mu?")]
    [SerializeField] private bool _isParticleEffect;
    [Tooltip("Patlamanýn particle effectid")]
    [SerializeField] private GameObject _explosionParticle;
    [Tooltip("Objenin parçalanabilir hali")]
    [SerializeField] private GameObject _smashableObject;
    [Tooltip("Patlamanýn yarýçapý")]
    [SerializeField] private float _explosionRadius = 5;
    [Tooltip("Patlamanýn gücü")]
    [SerializeField] private float _explosionForce = 500;
    [Tooltip("Patlayýcý objenin dayanýklýlýðý")]
    [SerializeField] private float _durability;

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
        Smash(_smashableObject);
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
    /// Objenin dayanýklýk deðerini azaltýr
    /// </summary>
    /// <param name="collision"></param>
    public void DoDamage(Collision collision)
    {
        var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        _durability -= collisionForce;
    }

    /// <summary>
    /// Parçalamannýn gerçekleþebilirliðini kontrol eder
    /// </summary>
    /// <param name="collision"></param>
    public bool CheckSmash()
    {
        if (_durability <= 0) return true;
        else return false;

    }

    public void Smash(GameObject smashableObject)
    {
        if (_smashableObject != null)
        {
            Instantiate(_smashableObject, transform.position, Quaternion.identity);
        }
    }
}
