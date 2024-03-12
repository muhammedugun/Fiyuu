// Refactor 12.03.24

using System;
using UnityEngine;
using MoreMountains.Feedbacks;
public class Enemy : DamagableObjectBase
{
    /// <summary>
    /// Öldü eventi
    /// </summary>
    public static event Action OnDied;

    [SerializeField] private GameObject visual;
    [SerializeField] private MMF_Player dieFeedback;
    [SerializeField] private MMF_Player damageFeedback;

    internal Animator animator;

    private bool _isDead;

    private void Awake()
    {
        _massMultiplier = 1f;
    }

    protected override void Start()
    {
        base.Start();
        gameObject.tag = "Enemy";
        animator = GetComponent<Animator>();
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
    /// Ölmeyi gerçekleþtir
    /// </summary>
    /// <param name="smashableObject">Objenin parçalanabilir halinin örneði</param>
    private void Die()
    {
        _isDead = true;
        OnDied?.Invoke();
        dieFeedback?.PlayFeedbacks();
        if(animator!=null)
            animator.SetTrigger("death");
    }


    public void SetActiveFalse()
    {
        visual.SetActive(false);
        GetComponent<CapsuleCollider>().enabled = false;
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        this.enabled = false;
    }

    /// <summary>
    /// Ölünebilir mi? diye kontrol et
    /// </summary>
    /// <param name="collision"></param>
    private bool CheckDie()
    {
        if (!_isDead && durability <= 0) return true;
        else return false;
    }


}
