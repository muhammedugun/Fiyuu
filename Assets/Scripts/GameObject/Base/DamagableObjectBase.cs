// Refactor 23.08.24
using UnityEngine;

/// <summary>
/// Hasar alabilen objelerin ana sýnýfýdýr
/// </summary>
public abstract class DamagableObjectBase : RigidObjectBase
{
    /// <summary>
    /// Objenin dayanýklýlýk deðerini tutar. Obje hasar aldýðýnda bu deðer azalýr
    /// </summary>
    public float durability { get; set; }
    /// <summary>
    /// Hasar hassaslýðý, objenin ne kadarlýk bir fiziksel etkiye uðradýktan sonra hasar alabileceðini belirler
    /// </summary>
    [SerializeField] protected int damageSensitivity;
    /// <summary>
    /// Dayanýklýlýk çarpaný, objenin dayanýklýlýðý bu deðerle çarpýldýktan sonra atanýr. 
    /// Bu deðer ne kadar yüksek olursa o kadar dayanýklý olur
    /// </summary>
    protected float _durabilityMultiplier=1f;

    protected override void Start()
    {
        base.Start();
        AssignDurability(_durabilityMultiplier);
    }

    /// <summary>
    /// Objenin aðýrlýðýný ve parametredeki deðeri baz alarak dayanýklýlýðýný atar
    /// </summary>
    protected virtual void AssignDurability(float durabilityMultiplier = 1f)
    {
        durability = durabilityMultiplier * _rigidbody.mass;
    }

    /// <summary>
    /// Objenin dayanýklýk deðerini azaltýr
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
