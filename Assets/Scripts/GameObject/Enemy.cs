// Refactor 23.08.24
using UnityEngine;
using MoreMountains.Feedbacks;
using System.Collections.Generic;

/// <summary>
/// D��manlar i�in ana s�n�f
/// </summary>
public class Enemy : DamagableObjectBase
{
    public float durabilityMultiplier = 50f;
    public float massMultiplier = 1f;

    [SerializeField] private GameObject _visual;
    [SerializeField] private MMF_Player _dieFeedback;
    [SerializeField] private MMF_Player _damageFeedback;
    [SerializeField] private MMF_Player _skipBeginningFeedback;

    private bool _isDead = false;
    private Animator _animator;

    private void Awake()
    {
        base._massMultiplier = massMultiplier;
        base._durabilityMultiplier = durabilityMultiplier;
    }

    protected override void Start()
    {
        _animator = GetComponent<Animator>();
        base.Start();
        gameObject.tag = "Enemy";
    }

    private void OnEnable()
    {
        EventBus.Subscribe(EventType.FirstClickInLevel, SkipBeginning);
    }
    private void OnDisable()
    {
        EventBus.Subscribe(EventType.FirstClickInLevel, SkipBeginning);
        CancelInvoke();
    }


    private void OnCollisionEnter(Collision collision)
    {
        var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        if (durability > 0 && collisionForce > damageSensitivity)
        {
            _damageFeedback.PlayFeedbacks();
        }

        DoDamage(collision);

        if (CheckDie())
            Die();
    }

    public override void DoDamage(Collision collision, float damageMultiplier = 1f)
    {
        base.DoDamage(collision, damageMultiplier);
    }

    /// <summary>
    /// Ba�lang�� feedback'lerini atla
    /// </summary>
    public void SkipBeginning()
    {
        _skipBeginningFeedback.PlayFeedbacks();
        EventBus.Unsubscribe(EventType.FirstClickInLevel, SkipBeginning);
    }

    public void SetEnableAnimator()
    {
        _animator.enabled = true;
    }

    /// <summary>
    /// �lmeyi ger�ekle�tir
    /// </summary>
    /// <param name="smashableObject">Objenin par�alanabilir halinin �rne�i</param>
    private void Die()
    {
        _isDead = true;
        EventBus.Publish(EventType.EnemyDied);
        _dieFeedback?.PlayFeedbacks(this.transform.position, ScoreManager.enemyScore);
        Invoke(nameof(DestroyCharacter), 0.7f);
    }

    private void DestroyCharacter()
    {
        Destroy(gameObject);
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
