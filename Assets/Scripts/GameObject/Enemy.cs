using System;
using UnityEngine;
using UnityEngine.Networking.Types;

public class Enemy : MonoBehaviour, IDamageable
{
    
    public float Durability { get { return _durability; } set { _durability = value; } }
    [SerializeField] private float _durability;
    public static event Action OnDied;

    private void Awake()
    {
        Durability = _durability;
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        DoDamage(collision);
        if (CheckDie()) Die();
    }

    public void DoDamage(Collision collision)
    {
        var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        _durability -= collisionForce;
    }

    /// <summary>
    /// Ölmeyi gerçekleþtir
    /// </summary>
    /// <param name="smashableObject">Objenin parçalanabilir halinin örneði</param>
    private void Die()
    {
        OnDied.Invoke();
        Debug.Log(transform.name + " adlý düþman öldü");
        gameObject.SetActive(false);

    }

    /// <summary>
    /// Ölünebilir mi? diye kontrol et
    /// </summary>
    /// <param name="collision"></param>
    private bool CheckDie()
    {
        if (_durability <= 0) return true;
        else return false;
    }


}
