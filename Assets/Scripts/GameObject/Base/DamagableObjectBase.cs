// Refactor 12.03.24

using UnityEngine;

public abstract class DamagableObjectBase : RigidObjectBase
{
    public float durability { get; set; }

    protected float _durabilityMultiplier=1f;

    protected override void Start()
    {
        base.Start();
        AssignDurability(_durabilityMultiplier);
    }

    /// <summary>
    /// Objenin dayanıklılığını atar
    /// </summary>
    protected virtual void AssignDurability(float durabilityMultiplier = 1f)
    {
        durability = durabilityMultiplier * _rigidbody.mass;
    }

    /// <summary>
    /// Objenin dayanıklık değerini azaltır
    /// </summary>
    /// <param name="collision"></param>
    public virtual void DoDamage(Collision collision, float damageMultiplier = 1f)
    {
        var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        Debug.Log("colforce: " + collisionForce);
        if (durability > 0 && collisionForce / _rigidbody.mass > 100f)
        {
            durability -= collisionForce * damageMultiplier;
            if (durability < 0)
            {
                durability = 0;
            }
        }
    }


    
}
