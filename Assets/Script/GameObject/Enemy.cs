using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.Events;
using MoreMountains.Feedbacks;
public class Enemy : MonoBehaviour, IDamageable
{
    
    [SerializeField] private GameObject body;
    [Header("Feedbacks")]
    [SerializeField] private MMF_Player dieFeedback;
    [SerializeField] private MMF_Player damageFeedback;
    public static event Action OnDied;
    public float Durability { get { return _durability; } set { _durability = value; } }
    [SerializeField] private float _durability;
    internal Animator animator;
    private bool _isDead;

    private void Awake()
    {
        Durability = _durability;
    }

    private void Start()
    {
        gameObject.tag = "Enemy";

        animator = GetComponent<Animator>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        DoDamage(collision);
        if (!_isDead && CheckDie()) Die();
    }

    public void DoDamage(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<Ammo>(out Ammo ammo))
            damageFeedback.PlayFeedbacks();
        var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        _durability -= collisionForce;
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
        body.SetActive(false);
        GetComponent<CapsuleCollider>().enabled = false;
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        this.enabled = false;
        //gameObject.SetActive(false);
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
