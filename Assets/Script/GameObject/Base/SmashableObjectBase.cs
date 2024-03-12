// Refactor 12.03.24
using UnityEngine;

[RequireComponent(typeof(Fracture))]
public abstract class SmashableObjectBase : DamagableObjectBase
{
    protected Fracture _fracture;
    /// <summary>
    /// Parçalanma gerçekleşti mi?
    /// </summary>
    protected bool _isSmash;

    protected override void Start()
    {
        base.Start();
        _fracture = GetComponent<Fracture>();
    }

    /// <summary>
    /// Parçalamayı gerçekleştirir
    /// </summary>
    public virtual void Smash(Collision collision)
    {
        if (collision.contactCount > 0)
        {
            _isSmash = true;
            var contact = collision.contacts[0];
            _fracture.callbackOptions.CallOnFracture(contact.otherCollider, gameObject, contact.point);
            _fracture.ComputeFracture();
        }
    }

    /// <summary>
    /// Parçalamanın gerçekleşebilirliğini kontrol eder
    /// </summary>
    /// <param name="collision"></param>
    public bool CheckSmash()
    {
        if (!_isSmash && durability <= 0)
            return true;
        else
            return false;
    }
}
