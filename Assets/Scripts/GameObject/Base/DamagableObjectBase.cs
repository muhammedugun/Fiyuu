// Refactor 12.03.24

using UnityEngine;

public abstract class DamagableObjectBase : RigidObjectBase
{
    public float durability { get; set; }

    [SerializeField] protected int damageSensitivity;

    protected float _durabilityMultiplier=1f;

    protected override void Start()
    {
        base.Start();
        AssignDurability(_durabilityMultiplier);
    }

    /// <summary>
    /// Objenin dayanýklýlýðýný atar
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
