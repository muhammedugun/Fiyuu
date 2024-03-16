// Refactor 12.03.24

using System;
using UnityEngine;
using MoreMountains.Feedbacks;
public class Enemy : DamagableObjectBase
{
    /// <summary>
    /// �ld� eventi
    /// </summary>
    public static event Action OnDied;

    [SerializeField] private GameObject visual;
    [SerializeField] private MMF_Player dieFeedback;
    [SerializeField] private MMF_Player damageFeedback;
    [SerializeField] private float durabilityMultiplier=50f;

    private bool _isDead;

    private void Awake()
    {
        _massMultiplier = 1f;
        _durabilityMultiplier = durabilityMultiplier;
    }

    protected override void Start()
    {
        base.Start();
        gameObject.tag = "Enemy";
    }

    private void OnCollisionEnter(Collision collision)
    {
        DoDamage(collision);
        if (collision.gameObject.TryGetComponent<Ammo>(out Ammo ammo))
            damageFeedback.PlayFeedbacks();
        if (CheckDie()) 
            Die();
    }

    /// <summary>
    /// �lmeyi ger�ekle�tir
    /// </summary>
    /// <param name="smashableObject">Objenin par�alanabilir halinin �rne�i</param>
    private void Die()
    {
        _isDead = true;
        dieFeedback?.PlayFeedbacks();
        OnDied?.Invoke();
    }

    /// <summary>
    /// �l�nebilir mi? diye kontrol et
    /// </summary>
    /// <param name="collision"></param>
    private bool CheckDie()
    {
        if (!_isDead && durability <= 0) return true;
        else return false;
    }


}
