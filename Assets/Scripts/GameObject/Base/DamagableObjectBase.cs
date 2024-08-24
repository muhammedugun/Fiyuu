// Refactor 23.08.24
using UnityEngine;

/// <summary>
/// Hasar alabilen objelerin ana s�n�f�d�r
/// </summary>
public abstract class DamagableObjectBase : RigidObjectBase
{
    /// <summary>
    /// Objenin dayan�kl�l�k de�erini tutar. Obje hasar ald���nda bu de�er azal�r
    /// </summary>
    public float durability { get; set; }
    /// <summary>
    /// Hasar hassasl���, objenin ne kadarl�k bir fiziksel etkiye u�rad�ktan sonra hasar alabilece�ini belirler
    /// </summary>
    [SerializeField] protected int damageSensitivity;
    /// <summary>
    /// Dayan�kl�l�k �arpan�, objenin dayan�kl�l��� bu de�erle �arp�ld�ktan sonra atan�r. 
    /// Bu de�er ne kadar y�ksek olursa o kadar dayan�kl� olur
    /// </summary>
    protected float _durabilityMultiplier=1f;

    protected override void Start()
    {
        base.Start();
        AssignDurability(_durabilityMultiplier);
    }

    /// <summary>
    /// Objenin a��rl���n� ve parametredeki de�eri baz alarak dayan�kl�l���n� atar
    /// </summary>
    protected virtual void AssignDurability(float durabilityMultiplier = 1f)
    {
        durability = durabilityMultiplier * _rigidbody.mass;
    }

    /// <summary>
    /// Objenin dayan�kl�k de�erini azalt�r
    /// </summary>
    /// <param name="collision"></param>
    public virtual void DoDamage(Collision collision, float damageMultiplier = 1f)
    {
        var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;

        if (durability > 0 && collisionForce > damageSensitivity)
        {
            durability -= collisionForce * damageMultiplier;
            if (durability < 0)
            {
                durability = 0;
            }
        }
    }


    
}
