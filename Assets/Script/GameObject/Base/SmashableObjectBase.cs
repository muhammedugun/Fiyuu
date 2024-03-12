// Refactor 12.03.24
using UnityEngine;

[RequireComponent(typeof(Fracture))]
public abstract class SmashableObjectBase : DamagableObjectBase
{
    protected Fracture _fracture;
    /// <summary>
    /// Par�alanma ger�ekle�ti mi?
    /// </summary>
    protected bool _isSmash;

    protected override void Start()
    {
        base.Start();
        _fracture = GetComponent<Fracture>();
    }

    /// <summary>
    /// Par�alamay� ger�ekle�tirir
    /// </summary>
    public virtual void Smash(Collision collision)
    {
        if (collision.contactCount > 0)
        {
            var contact = collision.contacts[0];
            _fracture.callbackOptions.CallOnFracture(contact.otherCollider, gameObject, contact.point);
            _fracture.ComputeFracture();
        }
    }

    /// <summary>
    /// Par�alaman�n ger�ekle�ebilirli�ini kontrol eder
    /// </summary>
    /// <param name="collision"></param>
    public bool CheckSmash()
    {
        if (durability <= 0)
            return true;
        else
            return false;
    }
}
