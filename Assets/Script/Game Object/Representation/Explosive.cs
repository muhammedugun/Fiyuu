using UnityEngine;

/// <summary>
/// Patlayýcý ile ilgili görevlerden sorumlu
/// </summary>
public class Explosive : MonoBehaviour
{
    [SerializeField] private GameObject _particles, _smashableObject;
    [Tooltip("Patlamanýn yarýçapý")]
    [SerializeField] private float _explosionRadius = 5;
    [Tooltip("Patlamanýn gücü")]
    [SerializeField] private float _explosionForce = 500;
    [Tooltip("Patlayýcý objenin dayanýklýlýðý")]
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
    /// Objenin dayanýklýk deðerini azaltýr
    /// </summary>
    /// <param name="collision"></param>
    private void Damage(Collision collision)
    {
        var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        _durability -= collisionForce;
    }

    /// <summary>
    /// Parçalamannýn gerçekleþebilirliðini kontrol eder
    /// </summary>
    /// <param name="collision"></param>
    private bool CheckSmash()
    {
        if (_durability <= 0) return true;
        else return false;

    }
}
