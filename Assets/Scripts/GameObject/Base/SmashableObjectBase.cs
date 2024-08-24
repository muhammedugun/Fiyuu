// Refactor 23.08.24
using UnityEngine;

/// <summary>
/// Parçalanabilir objeler için ana sýnýftýr
/// </summary>
[RequireComponent(typeof(Fracture))]
public abstract class SmashableObjectBase : DamagableObjectBase
{
    /// <summary>
    /// Parçalanma gerçekleþti mi?
    /// </summary>
    protected bool _isSmash;

    private Fracture _fracture;

    protected override void Start()
    {
        base.Start();
        _fracture = gameObject.GetComponent<Fracture>();
    }

    /// <summary>
    /// Parçalamayý gerçekleþtirir
    /// </summary>
    public virtual void Smash(Collision collision)
    {
        if (collision.contactCount > 0)
        {
            _isSmash = true;
            var contact = collision.contacts[0];
            if(_fracture==null)
            {
                Debug.Log("Fracture is null");
            }
            _fracture.callbackOptions.CallOnFracture(contact.otherCollider, gameObject, contact.point);
            _fracture.ComputeFracture();
        }
    }

    /// <summary>
    /// Parçalamanýn gerçekleþebilirliðini kontrol eder
    /// </summary>
    public bool CheckSmash()
    {
        if (!_isSmash && durability <= 0)
            return true;
        else
            return false;
    }
}
