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
    [SerializeField] private float durabilityMultiplier=50f;
    [SerializeField] private float massMultiplier = 1f;

    private bool _isDead;
    private Animator _animator;
    private void Awake()
    {
        _massMultiplier = massMultiplier;
        _durabilityMultiplier = durabilityMultiplier;
    }

    protected override void Start()
    {
        _animator = GetComponent<Animator>();
        base.Start();
        gameObject.tag = "Enemy";


    }

    

    private void OnCollisionEnter(Collision collision)
    {
        DoDamage(collision);
        damageFeedback.PlayFeedbacks();
        if (CheckDie()) 
            Die();
    }

    public void SetEnableAnimator()
    {
        _animator.enabled = true;
    }
    /// <summary>
    /// Ölmeyi gerçekleþtir
    /// </summary>
    /// <param name="smashableObject">Objenin parçalanabilir halinin örneði</param>
    private void Die()
    {
        _isDead = true;
        OnDied?.Invoke();
        dieFeedback?.PlayFeedbacks(this.transform.position, 200);
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
