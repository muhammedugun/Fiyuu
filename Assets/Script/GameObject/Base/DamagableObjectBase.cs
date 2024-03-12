// Refactor 12.03.24

using UnityEngine;

public abstract class DamagableObjectBase : RigidObjectBase
{
    public float durability { get; set; }

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
        durability -= collisionForce * damageMultiplier;
    }
}
