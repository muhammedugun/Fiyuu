// Refactor 23.08.24
using UnityEngine;

/// <summary>
/// Patlayıcı nesne ile ilgili görevlerden sorumlu ana sınıf
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
            float explosionForce = _explosionForce;
            var rb = obj.GetComponent<Rigidbody>();
            if (rb == null) { continue; }

            if (rb.TryGetComponent(out SmashableObjectBase smashableObject))
            {
                // Ben mühimmatsam ve obj bir yapıysa
                if (this.gameObject.CompareTag("Ammo") && obj.gameObject.CompareTag("Building"))
                {
                    var building = obj.gameObject.GetComponent<Building>();
                    var ammo = gameObject.GetComponent<Ammo>();
                    if (Building.armorStrengths[(int)building.armor - 1, (int)ammo.matter - 1] == 0)
                    {
                        var differentPosition = rb.transform.position - transform.position;
                        smashableObject.durability -= (explosionForce * collision.relativeVelocity.magnitude * 300) / differentPosition.magnitude;
                    }
                    if (Building.armorStrengths[(int)building.armor - 1, (int)ammo.matter - 1] == 1)
                    {
                        explosionForce = 0f;
                    }
                }
                else
                {
                    var differentPosition = rb.transform.position - transform.position;
                    smashableObject.durability -= (explosionForce * collision.relativeVelocity.magnitude * 300) / differentPosition.magnitude;
                }

            }

            // Ben wood mühimmatıysam ve Stone olan yapıya çarptıysam ve şuanda obj'de bir mühimmat parçası ise
            else if (this.gameObject.CompareTag("Ammo") && collision.gameObject.CompareTag("Building") && obj.CompareTag("Ammo"))
            {
                var ammo = gameObject.GetComponent<Ammo>();

                if (Ammo.CheckWoodVersusStone(collision.gameObject, ammo.matter))
                {
                    var objAmmo = obj.GetComponent<Ammo>();
                    if (objAmmo == null)
                    {
                        obj.GetComponent<Rigidbody>().mass = 0.01f;
                        explosionForce = 0.01f;
                    }
                }
            }

            rb.AddExplosionForce(explosionForce * collision.relativeVelocity.magnitude, transform.position, _explosionRadius, 0.0f, ForceMode.Impulse);
        }
        
    }

}
